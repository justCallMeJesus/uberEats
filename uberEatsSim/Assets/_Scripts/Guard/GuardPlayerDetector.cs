using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;




public class GuardPlayerDetector : MonoBehaviour
{
    // THIS SCRIPT IS NO LONGER IN USE
    [SerializeField] private GuardAI guard;
    
    


    [SerializeField] private float fovDistance = 2.3f;
    [SerializeField][Range(0, 360)] private float fovAngle;

    



    [SerializeField] private float detectionSpeed = 5;
    [SerializeField] private float distanceDrawbackMultiplier = 5;
    [SerializeField] private float alertDecay = 5;
    [SerializeField] private float alertedSeardchingDecay = 2;
    [SerializeField] private LayerMask ignoreMask;


    private Vector3 raycastOffset = new Vector3 (0, 0.5f, 0);


    private void Awake()
    {

    }

    private void Start()
    {
        guard.alertState = AlertState.Patrolling;
        guard.alertLevel = 0;

    }

    private void Update()
    {
        
        //UpdateAlertStage(PlayerInFOV());
    }

    public bool PlayerInFOV()
    {
        bool playerInFOV = false;
        Collider[] targetsInFOV = Physics.OverlapSphere(transform.position, fovDistance);
        foreach (Collider target in targetsInFOV)
        {
            if (target.CompareTag("Player"))
            {
                float signedAngle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
                if (Mathf.Abs(signedAngle) < fovAngle / 2 && Physics.Raycast(transform.position + raycastOffset, target.transform.position - transform.position, out RaycastHit hit, fovDistance, ~ignoreMask))
                {
                    Debug.Log(hit.collider.name);
                    Debug.DrawLine(transform.position + raycastOffset, hit.point);
                    if (hit.collider.TryGetComponent(out PlayerMovement player))
                    {
                        return true;
                    }
                }
                break;
            }
        }
        return false;
    }

    private void UpdateAlertStage(bool playerInFOV)
    {
        switch (guard.alertState)
        {
            case AlertState.Patrolling:
                if (playerInFOV)
                {
                    guard.ChangeAlertState(AlertState.Suspicious);
                }
                break;
            case AlertState.Suspicious:
                if (playerInFOV)
                {
                    guard.alertLevel += Time.deltaTime * detectionSpeed * (distanceDrawbackMultiplier / Vector3.Distance(transform.position, guard.player.transform.position));
                    if(guard.alertLevel >= 100)
                    {
                        guard.ChangeAlertState(AlertState.Alerted);
                    }
                }
                else
                {
                    guard.alertLevel -= Time.deltaTime * alertDecay;
                    if(guard.alertLevel <= 0)
                    {
                        guard.ChangeAlertState(AlertState.Patrolling);
                    }
                }
                break;
            case AlertState.Alerted:
                if(!playerInFOV)
                {
                    guard.ChangeAlertState(AlertState.Searching);
                    //guard.lastKnownPlayerLocation = guard.player.transform.position;
                }
                break;
            case AlertState.Searching:
                if(playerInFOV)
                {
                    guard.ChangeAlertState(AlertState.Alerted);
                    guard.alertLevel = guard.alertLevel = 100;
                }
                else
                {
                    guard.alertLevel -= Time.deltaTime * alertDecay;
                }
                break;
        }
    }







    // Gizoms #######################################


    //private void OnDrawGizmos()
    //{
    //    Color color = new Color(0, 1, 0, 0.3f);
    //    if (guard.alertState == AlertState.Suspicious)
    //    {
    //        color = Color.Lerp(new Color(0, 1, 0, 0.3f), new Color(1, 0, 0, 0.3f), guard.alertLevel / 100f);
    //    }
    //    else if (guard.alertState == AlertState.Alerted)
    //    {
    //        color = new Color(1, 0, 0, 0.3f);
    //    }


    //    Handles.color = color;
    //    Handles.DrawSolidArc(transform.position, transform.up, Quaternion.AngleAxis(-fovAngle / 2f, transform.up) * transform.forward, fovAngle, fovDistance);
    //}
        
}
