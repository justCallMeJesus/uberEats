using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 5f;
    private float interactionHeight = 1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
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

    private void VehicleInteraction(Vehicle vehicle)
    {
        vehicle.EnterVehicle(this.gameObject);
    }


}



