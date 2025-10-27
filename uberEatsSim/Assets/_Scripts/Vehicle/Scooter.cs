using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scooter : Vehicle
{

    [SerializeField] private float scooterSpeed = 20f;




    // Start is called before the first frame update
    private void Awake()
    {
        interacterPositionOffset = Vector3.zero;
        vehicleSpeed = scooterSpeed;
    }

    // Update is called once per frame
    private void Update()
    {
        base.Update();
    }
}
