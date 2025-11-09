using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Vehicle : MonoBehaviour, IInteractable
{

    protected float vehicleSpeed;
    protected float vehicleRotateSpeed = 25f;

    protected Vector3 interacterPositionOffset = Vector3.zero;

    private GameInput gameInput;
    public Player interacter;
    private CharacterController cc;

    public event Action OnPlayerEntered;

    // Start is called before the first frame update
    protected void Start()
    {
        gameInput = GameInput.instance;
        cc = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    public void Update()
    {
        if (gameInput.playerInputActions.Vehicle.enabled && interacter != null)
        {
            MoveVehicle();
            Debug.Log("moveVehicle");

        }
    }

    public void EnterVehicle(Player interacter)
    {
        Vector3 playersPosition = interacter.transform.position;
        Quaternion playersRotation = interacter.transform.rotation;
        interacter.transform.position = transform.position + interacterPositionOffset;
        interacter.transform.rotation = transform.rotation;
        interacter.transform.SetParent(transform);
        transform.position = playersPosition;
        transform.rotation = playersRotation;
        this.interacter = interacter;
        interacter.PlayerColliderEnabled(false);
        GameInput.instance.EnableVehicleControls();
        OnPlayerEntered?.Invoke();

        gameInput.playerInputActions.Vehicle.Exit.performed += Exit_performed;
    }

    private void ExitVehicle()
    {
        gameInput.playerInputActions.Vehicle.Exit.performed -= Exit_performed;
        interacter.transform.SetParent(null);
        interacter.PlayerColliderEnabled(true);
        this.interacter = null;
        GameInput.instance.DisableVehicleControls();
    }

    private void Exit_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        ExitVehicle();
    }

    private void MoveVehicle()
    {
        Vector2 inputVector = gameInput.playerInputActions.Vehicle.Movement.ReadValue<Vector2>().normalized;
        Vector3 moveDirDistance = new Vector3(inputVector.x, 0f, inputVector.y) * Time.deltaTime * vehicleSpeed;

        if (moveDirDistance.sqrMagnitude != 0)
        {
            cc.Move(moveDirDistance);
            transform.forward = Vector3.Slerp(transform.forward, moveDirDistance, Time.deltaTime * vehicleRotateSpeed);
        }
    }


    public GameObject GetGameObject()
    {
        return this.gameObject;
    }
}
