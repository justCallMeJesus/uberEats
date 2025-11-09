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
        startPos = transform.parent.parent.position;
        Debug.Log(startPos);
        yield return new WaitForSeconds(1.0f);
        Debug.Log(transform.parent.parent.position);
        if (startPos != transform.parent.parent.position)
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
