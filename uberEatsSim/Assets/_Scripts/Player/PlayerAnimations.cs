using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAnimations : MonoBehaviour
{
    Animator animator;
    private GameInput gameInput;
    void Start()
    {
        animator = GetComponent<Animator>();
        gameInput = GameInput.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameInput.playerInputActions.Player.Movement.ReadValue<Vector2>() != Vector2.zero && transform.parent.parent == null)
        {
            animator.SetBool("isWalking", true);
        }
        else if (gameInput.playerInputActions.Player.Movement.ReadValue<Vector2>() == Vector2.zero && transform.parent.parent == null) //no movement and no scooter
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("onScooter", false);
        }  
        else if (gameInput.playerInputActions.Player.Movement.ReadValue<Vector2>() == Vector2.zero && transform.parent.parent != null) //no movement and scooter
        {
            animator.SetBool("onScooter", true);
        }
        else if (gameInput.playerInputActions.Player.Movement.ReadValue<Vector2>() != Vector2.zero && transform.parent.parent != null) //movement and scooter
        {
            animator.SetBool("onScooter", true);
        }
        
    }
}
