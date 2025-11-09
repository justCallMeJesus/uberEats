using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public PlayerInteraction playerInteraction;
    [HideInInspector]
    public PlayerMovement playerMovement;

    
    public VehicleDetector vehicleDetector;

    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private CharacterController characterController;
    private void Awake()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
        playerMovement = GetComponent<PlayerMovement>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void PlayerColliderEnabled(bool isEnabled)
    {
        characterController.enabled = isEnabled;
    }



    
}
