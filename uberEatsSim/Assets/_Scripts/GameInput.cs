using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    public PlayerInputActions playerInputActions { get; private set; }

    public static GameInput instance { get; private set; }


    // ========= OutgoingEvents ===================
    // List controls
    public event Action OnShoppingListPressed;
    public event Action OnShoppingListReleased;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one GameInput in the Scene");
        }
        instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.GeneralInGame.Enable();

        playerInputActions.GeneralInGame.OpenList.performed += OpenList_performed;
        playerInputActions.GeneralInGame.OpenList.canceled += OpenList_canceled;
    }

    private void OpenList_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
        OnShoppingListReleased?.Invoke();
    }

    private void OpenList_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {

        OnShoppingListPressed?.Invoke();
    }

    public void EnableVehicleControls()
    {
        playerInputActions.Player.Disable();
        playerInputActions.Vehicle.Enable();
    }

    public void DisableVehicleControls()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Vehicle.Disable();
    }
    

    public Vector2 GetMovementVectorNormalized(Vector2 inputVector)
    {

        inputVector = inputVector.normalized;

        return inputVector;
    }

    public Vector2 GetMovementVectorOriginal()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        return inputVector;
    }
}
