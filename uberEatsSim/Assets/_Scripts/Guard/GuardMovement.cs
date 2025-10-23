using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardMovement : MonoBehaviour
{
    private Vector3 playersLastKnownLocation;


    private NavMeshAgent agent;
    private GuardAI guard;


    private Transform placeToGo;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        guard = GetComponent<GuardAI>();
    }

    private void GuardMovementPerform()
    {
        switch (guard.alertState)
        {
            case AlertState.Patrolling:
                break;
            case AlertState.Suspicious:
                break;
            case AlertState.Alerted:
                agent.SetDestination(guard.player.transform.position);
                break;
            case AlertState.Searching:
                playersLastKnownLocation = guard.player.transform.position;
                //agent.SetDestination(guard.lastKnownPlayerLocation);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //GuardMovementPerform();
    }


}
