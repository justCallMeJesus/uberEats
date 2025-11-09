using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public float moveSpeed = 7f;
    private float interactionMoveSpeed = 0.5f;
    private float rotateSpeed = 25f;

    private GameInput gameInput;

    [SerializeField] private Vehicle vehicle;

    private CharacterController cc;

    private float originalSpeed;

    

    private void Awake()
    {
        originalSpeed = moveSpeed;
    }



    // Start is called before the first frame update
    void Start()
    {
        gameInput = GameInput.instance;

        cc = GetComponent<CharacterController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInput.playerInputActions.Player.enabled)
        {

            MovePlayer();
            

        }

        
    }

    private void MovePlayer()
    {
        Vector2 inputVector = gameInput.playerInputActions.Player.Movement.ReadValue<Vector2>().normalized;
        
        Vector3 moveDirDistance = new Vector3(inputVector.x, 0f, inputVector.y) * Time.deltaTime * moveSpeed;

        if (moveDirDistance.sqrMagnitude != 0) 
        {
            cc.Move(moveDirDistance);
            transform.forward = Vector3.Slerp(transform.forward, moveDirDistance, Time.deltaTime * rotateSpeed);
        }
        
    }

    public bool IsMoving()
    {
        return gameInput.playerInputActions.Player.Movement.ReadValue<Vector2>() != Vector2.zero;
    }

    public void SetInteractionMoveSpeed()
    {
        moveSpeed = interactionMoveSpeed;
    }

    public void SetNormalMoveSpeed()
    {
        moveSpeed = originalSpeed;
    }



    
}
