using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScooterCar :Vehicle
{
    [SerializeField] private WheelCollider frontWheel;
    [SerializeField] private WheelCollider backWheel;

    public float acceleration = 500f;
    public float breakingForce = 300f;

    public float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;

    public float maxSpeed = 20f;
    private float maxSpeedSqr;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        maxSpeedSqr = maxSpeed * maxSpeed;
        base.OnPlayerEntered += ScooterCar_OnPlayerEntered;
    }

    private void ScooterCar_OnPlayerEntered()
    {
        frontWheel.motorTorque = 300f;
    }

    private void FixedUpdate()
    {
        if(interacter != null)
        {
            currentAcceleration = acceleration * Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentBreakForce = breakingForce;
            }
            else
            {
                currentBreakForce = 0;
            }


            frontWheel.motorTorque = currentAcceleration;

            frontWheel.brakeTorque = currentBreakForce;
            backWheel.brakeTorque = currentBreakForce;

            if (interacter != null)
            {
                int horizontalInput = (int)Input.GetAxisRaw("Horizontal");
                currentTurnAngle = maxTurnAngle * horizontalInput;
            }
            else
            {
                currentTurnAngle = 0;
            }

            frontWheel.steerAngle = currentTurnAngle;


            float currentSpeedSqr = rb.velocity.sqrMagnitude;
            if (currentSpeedSqr > maxSpeedSqr)
            {
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
        

        
    }
    void Update()
    {
        base.Update();
    }



}
