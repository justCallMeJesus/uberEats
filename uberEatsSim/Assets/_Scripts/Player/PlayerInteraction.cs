using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static ItemSO;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 5f;
    private float interactionHeight = 1;
    private float detectionDistance = 5;


    public HashSet<Item> interactableItems = new HashSet<Item>();

    public Item currentlyHighlightedItem = null;
    private Item closestItem = null;

    private float pickupTime = 3f;
    private float timeHeld = 0f;
    private float exitTimeHeld = 0f;    
    private float exitTime = 2f;

    private Player player;

    public HashSet <Checkout> checkoutsInRange = new HashSet<Checkout>();
    public HashSet<ExitArea> exitAreasInRange = new HashSet<ExitArea>();
    private Checkout interactableCheckout = null;

    private bool currentlyCheckingOut = false;
    private ItemSO randomItemToCheckout = null;
    private bool pickingUp = false;

    public int testSelectAmount = 0;

    [SerializeField] private LayerMask interactionBlockLayers;
    [SerializeField] private LayerMask scooterLayer;

    private void Start()
    {
        player = GetComponent<Player>();
        GameManager.Instance.OnItemTypeCollected += Instance_OnItemTypeCollected;
        GameInput.instance.playerInputActions.Player.Interact.performed += Interact_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        TryInteract();
    }

    private void Instance_OnItemTypeCollected(GameManager.SelectedItems completedItem)
    {
        List<Item> itemsToRemove = interactableItems.Where(item => item.itemSO == completedItem.itemSO).ToList();
        foreach (Item itemToRemove in itemsToRemove)
        {
            interactableItems.Remove(itemToRemove);
            itemToRemove.DisableNormalHighlight();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    TryInteract();
        //}
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("space press started");
            if (GameManager.Instance.collectedItems.Count > 0)
            {
                randomItemToCheckout = GameManager.Instance.collectedItems[Random.Range(0, GameManager.Instance.collectedItems.Count)].itemSO;
            }

        }
        if (Input.GetKey(KeyCode.E))
        {
            TryPickUp();
            if(exitAreasInRange.Count > 0)
            {
                TryExit();
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("Space released");
            timeHeld = 0f;
            exitTimeHeld = 0f;
            UIManager.Instance.pickupBar.SetPickupBar(0);
            player.playerMovement.SetNormalMoveSpeed();
            pickingUp = false;
            //if (currentlyCheckingOut)
            //{
            //    currentlyCheckingOut = false;


            //}
        }
        if(!pickingUp)
        {
            HighlightClosestItemInRange();
        }


        testSelectAmount = interactableItems.Count;
        
        FindClosestCheckout();

        //if (player.playerMovement.IsMoving())
        //{
        //    timeHeld = 0f;
        //}

    }

    private void TryInteract()
    {
        //if (!Physics.Raycast(transform.position + new Vector3(0, interactionHeight, 0), transform.forward, out RaycastHit hit, interactionDistance, scooterLayer))
        //{
        //    return;
        //}
        //else
        //{
            
        //}
        
        //Debug.Log(hit.collider.name);

        //if (hit.collider.TryGetComponent(out IInteractable interactable))
        //{
        //    Debug.Log("interactable hit");
        //    if (interactable.GetGameObject().TryGetComponent(out Vehicle vehicle))
        //    {
        //        Debug.Log("interacted with vehicle");
        //        VehicleInteraction(vehicle);
        //    }

        //}
        
        if(player.vehicleDetector.vehicles.Count > 0)
        {
            Debug.Log(player.vehicleDetector.vehicles.First().gameObject.name);
            VehicleInteraction(player.vehicleDetector.vehicles.First());
        }
    }

    private void TryExit()
    {
        exitTimeHeld += Time.deltaTime;
        if(exitTimeHeld > exitTime)
        {
            Debug.Log("ExitedSupermarket");
            exitTimeHeld = 0f;
            GameManager.Instance.PlayerLeft();
        }
    }

    private void TryPickUp()
    {
        if(interactableCheckout != null)
        {
            player.playerMovement.SetInteractionMoveSpeed();
            currentlyCheckingOut = true;
            timeHeld += Time.deltaTime;
            if(randomItemToCheckout != null)
            {
                UIManager.Instance.checkoutBar.SetCheckoutBar(Mathf.RoundToInt((timeHeld / randomItemToCheckout.pickupTime) * 100));
                if (timeHeld > randomItemToCheckout.pickupTime)
                {
                    UIManager.Instance.checkoutBar.SetCheckoutBar(0);
                    player.playerMovement.SetNormalMoveSpeed();
                    GameManager.Instance.AddItemToPaid(randomItemToCheckout);
                    randomItemToCheckout = null;
                    if (GameManager.Instance.collectedItems.Count > 0)
                    {
                        randomItemToCheckout = GameManager.Instance.collectedItems[Random.Range(0, GameManager.Instance.collectedItems.Count)].itemSO;
                    }
                    timeHeld = 0f;
                }
            }
            
        }
        if(currentlyHighlightedItem != null)
        {
            pickingUp = true;
            player.playerMovement.SetInteractionMoveSpeed();
            timeHeld += Time.deltaTime;
            UIManager.Instance.pickupBar.SetPickupBar(Mathf.RoundToInt((timeHeld / currentlyHighlightedItem.itemSO.pickupTime) * 100));
            if (timeHeld > currentlyHighlightedItem.itemSO.pickupTime)
            {
                pickingUp = false;
                UIManager.Instance.pickupBar.SetPickupBar(0);
                player.playerMovement.SetNormalMoveSpeed();
                GameManager.Instance.ReduceItemCount(currentlyHighlightedItem.itemSO);
                interactableItems.Remove(currentlyHighlightedItem);
                Destroy(currentlyHighlightedItem.gameObject);
                timeHeld = 0f;
            }
            
        }
    }

    private void VehicleInteraction(Vehicle vehicle)
    {
        vehicle.EnterVehicle(player);
    }

    private void CheckForItemsInRadius()
    {
       // Physics.SphereCastAll(transform.position, detectionDistance)
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
    }

    // A method to find and return the closest GameObject
    public void FindClosestCheckout()
    {
        // Check if there a are checkouts in range
        if(checkoutsInRange.Count == 0)
        {
            interactableCheckout = null;
            UIManager.Instance.checkoutBar.gameObject.SetActive(false);
            return;
        }
        int unpaidItems = 0;
        foreach(var item in GameManager.Instance.collectedItems)
        {
            unpaidItems += item.count;
        }
        UIManager.Instance.SetUnpaidItemsAmount(unpaidItems);
        UIManager.Instance.checkoutBar.gameObject.SetActive(true);
        Checkout closestCheckout = null;
        float minDistance = Mathf.Infinity;

        Vector3 playerPosition = transform.position;

        foreach(Checkout checkout in checkoutsInRange)
        {
            if(checkout == null)
            {
                continue;
            }
            float currentDistanceSquared = (checkout.transform.position - playerPosition).sqrMagnitude;
            if (currentDistanceSquared < minDistance)
            {
                // 3. Update the minimum distance and store a reference to the closest object
                minDistance = currentDistanceSquared;
                closestCheckout = checkout;
            }
        }

        interactableCheckout = closestCheckout;
    }
    public Item FindClosestItem()
    {
        // Check if the list is empty or if the player reference is missing
        if (interactableItems == null || interactableItems.Count == 0 || interactableItems == null)
        {
            return null;
        }

        Item closestObject = null;
        float minDistance = Mathf.Infinity; // Initialize with a very large number

        // Get the player's position once for efficiency
        Vector3 playerPosition = transform.position;

        // Iterate through all potential targets
        foreach (Item target in interactableItems)
        {
            if(Physics.Raycast(transform.position, target.transform.position - transform.position, out RaycastHit hit, (target.transform.position - transform.position).magnitude, interactionBlockLayers))
            {
                continue;
            }
            // Ensure the target is valid (not null)
            if (target == null)
            {
                continue;
            }

            // 1. Calculate the squared distance. 
            //    Squared distance (Vector3.sqrMagnitude) is faster than regular distance (Vector3.Distance or Vector3.magnitude) 
            //    because it avoids calculating the square root. We only care about comparing *relative* distances.
            float currentDistanceSquared = (target.transform.position - playerPosition).sqrMagnitude;

            // 2. Check if this distance is the smallest one found so far
            if (currentDistanceSquared < minDistance)
            {
                // 3. Update the minimum distance and store a reference to the closest object
                minDistance = currentDistanceSquared;
                closestObject = target;
            }
        }

        // Return the object that had the smallest distance
        return closestObject;
    }

    private void HighlightClosestItemInRange()
    {
        // check if closest item is an item and set it to closest item
        if (FindClosestItem() is Item closestItem)
        {
            // check if closest item is currently not the highlighted item
            if (currentlyHighlightedItem != closestItem)
            {
                // if currentlyHighlightedItem is not null, enable normal highlight
                if (currentlyHighlightedItem != null) { currentlyHighlightedItem.EnableNormalHighlight(); }

                // set currentlyHighlightedItem as new closest item and enable its red highlight
                currentlyHighlightedItem = closestItem;
                closestItem.EnableClosestHighlight();
            }
            UIManager.Instance.pickupBar.gameObject.SetActive(true);
        }
        else if (currentlyHighlightedItem != null)
        {
            // if not closestItem found but currentlyHighlightedItem is not null, enable its normal highlight and set red highlight to null
            currentlyHighlightedItem.EnableNormalHighlight();
            currentlyHighlightedItem = null;
            UIManager.Instance.pickupBar.gameObject.SetActive(false);
        }
    }

    

}



