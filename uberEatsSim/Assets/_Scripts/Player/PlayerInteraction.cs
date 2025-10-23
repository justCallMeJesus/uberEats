using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 5f;
    private float interactionHeight = 1;
    private float detectionDistance = 5;


    public List<Item> interactableItems = new List<Item>();

    public Item currentlyHighlightedItem = null;
    private Item closestItem = null;

    private float pickupTime = 3f;
    private float timeHeld = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            TryPickUp();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space released");
            timeHeld = 0f;
        }
        HighlightClosestItemInRange();
        
    }

    private void TryInteract()
    {
        if (!Physics.Raycast(transform.position + new Vector3(0, interactionHeight, 0), transform.forward, out RaycastHit hit, interactionDistance))
        {
            return;
        }
        Debug.Log("collider hit");

        if (hit.collider.TryGetComponent(out IInteractable interactable))
        {
            Debug.Log("interactable hit");
            if (interactable.GetGameObject().TryGetComponent(out Vehicle vehicle))
            {
                Debug.Log("interacted with vehicle");
                VehicleInteraction(vehicle);
            }

        }
    }

    private void TryPickUp()
    {
        if(currentlyHighlightedItem != null)
        {
            Debug.Log("picking up");
            timeHeld += Time.deltaTime;
            if (timeHeld > currentlyHighlightedItem.itemSO.pickupTime)
            {
                Destroy(currentlyHighlightedItem.gameObject);
                timeHeld = 0f;
            }
            
        }
    }

    private void VehicleInteraction(Vehicle vehicle)
    {
        vehicle.EnterVehicle(this.gameObject);
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
        if (FindClosestItem() is Item closestItem)
        {
            if (currentlyHighlightedItem != closestItem)
            {
                if (currentlyHighlightedItem != null) { currentlyHighlightedItem.EnableHighlight(); }
                currentlyHighlightedItem = closestItem;
                closestItem.EnanbleClosestHighlight();
            }

        }
        else if (currentlyHighlightedItem != null)
        {
            currentlyHighlightedItem.EnableHighlight();
            currentlyHighlightedItem = null;
        }
    }

}



