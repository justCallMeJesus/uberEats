using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GuardAnimations : MonoBehaviour
{
    Animator animator;

    private Vector3 startPos;
 

   

    IEnumerator checkIfMoving()
    {
        startPos = transform.position;

        yield return new WaitForSeconds(0.1f);

        if (startPos != transform.position)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(checkIfMoving());
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    
}
