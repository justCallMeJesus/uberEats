using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleDetector : MonoBehaviour
{
    public HashSet<Vehicle> vehicles = new HashSet<Vehicle>();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if(other.TryGetComponent(out  Vehicle vehicle))
        {
            vehicles.Add(vehicle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Vehicle vehicle))
        {
            vehicles.Remove(vehicle);
        }
    }
}
