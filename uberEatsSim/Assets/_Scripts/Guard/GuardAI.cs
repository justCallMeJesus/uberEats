using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


public enum AlertState
{
    Patrolling,
    Suspicious,
    Alerted,
    Searching,
    Wandering,
    Returning
}

public class GuardAI : MonoBehaviour
{
    public Player player;
    public AlertState alertState;
    [Range(0, 100)] public float alertLevel;


    private GuardPlayerDetector playerDetector;

    // ============== Settings ==================

    [Header("Detection Settings")]
    [SerializeField] private float detectionSpeed = 5;
    [SerializeField] private float distanceDrawbackMultiplier = 5;
    [SerializeField] private float alertDecay = 5;
    [SerializeField] private float alertedSearchingDecay = 2;

    [Header("Field of view settings")]
    [SerializeField] private float fovDistance = 2.3f;
    [SerializeField] private float blindSightDistance = 0.4f;
    [SerializeField][Range(0, 360)] private float fovAngle;
    [SerializeField] private LayerMask ignoreMask;

    [Header("AI Settings")]
    [SerializeField] private float searchRadius = 5f;
    [SerializeField] private float searchDuration = 10f;
    [SerializeField] private float playerCatchRadius = 1f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float chasingSpeed = 6f;

    [Header("Other")]
    [SerializeField] private float rotationSpeed = 5f;
    //[SerializeField] public GuardPathSO guardPath;
    [SerializeField] public GuardWalkPoint[] guardWalkPoints;
    [SerializeField] public Vector3 spawnPoint;

    

    // ============ private Attributes =============

    private NavMeshAgent agent;
    private Vector3 raycastOffset = new Vector3(0, 0.5f, 0);
    private Vector3 lastKnownPlayerLocation;
    private float searchTimer;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private int currentPatrolDestination = 0;

    //public List<Vector3> guardPathLocations = new List<Vector3>();

    // =============== public Fields =================

    public float detectionMultiplier = 1;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        playerDetector = GetComponent<GuardPlayerDetector>();
        alertState = AlertState.Patrolling;
        startPosition = transform.position;
        startRotation = Quaternion.LookRotation(transform.forward);


    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AILogicLoop());
        player = FindObjectOfType<Player>();
    }

    private IEnumerator AILogicLoop()
    {
        while(true)
        {
            switch (alertState)
            {
                case AlertState.Patrolling:
                    PatrollingState();
                    break;
                case AlertState.Suspicious:
                    SuspiciousState();
                    break;
                case AlertState.Alerted:
                    AlertedState();
                    break;
                case AlertState.Searching:
                    SearchingState();
                    break;
                case AlertState.Wandering:
                    WanderingState();
                    break;
                case AlertState.Returning:
                    ReturningState();
                    break;
            }
            yield return null;
        }
    }

    // =========== States ============

    private void PatrollingState()
    {
        Debug.Log(transform.rotation.eulerAngles.y);
        if (guardWalkPoints != null)
        {
            if (guardWalkPoints.Length > 1)
            {
                if (agent.remainingDistance < 0.5)
                {
                    SetNextPatrolDestination();
                    agent.SetDestination(guardWalkPoints[currentPatrolDestination].location);
                }
            }
            else if (guardWalkPoints.Length == 1)
            {
                if (transform.rotation.eulerAngles.y != guardWalkPoints[0].rotation)
                {
                    
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(guardWalkPoints[0].rotation, Vector3.up), Time.deltaTime * rotationSpeed);
                }
            }
            
        }
        if (PlayerInFOV())
        {
            ChangeAlertState(AlertState.Suspicious);
        }
        if(alertLevel > 0)
        {
            alertLevel -= Time.deltaTime * alertDecay;
        }
    }

    private void SuspiciousState()
    {
        if (PlayerInFOV())
        {
            alertLevel += Time.deltaTime * detectionSpeed * (distanceDrawbackMultiplier / Vector3.Distance(transform.position, player.transform.position));
            if (alertLevel >= 100)
            {
                agent.speed = chasingSpeed;
                ChangeAlertState(AlertState.Alerted);
            }
        }
        else
        {
            alertLevel -= Time.deltaTime * alertDecay;
            if (alertLevel <= 0)
            {
                ChangeAlertState(AlertState.Patrolling);
            }
        }
    }

    private void AlertedState()
    {
        agent.SetDestination(player.transform.position);
        if (!PlayerInFOV())
        {
            ChangeAlertState(AlertState.Searching);
            lastKnownPlayerLocation = player.transform.position;
            StartCoroutine(ImprovePlayerTrack());
        }
        if(Vector3.Distance(transform.position, player.transform.position) < playerCatchRadius)
        {
            Debug.Log("Player caught");
            PlayerCaught();
        }
    }

    private IEnumerator ImprovePlayerTrack()
    {
        yield return new WaitForSeconds(1.5f);
        lastKnownPlayerLocation = player.transform.position;
        yield return new WaitForSeconds(1.5f);
        lastKnownPlayerLocation = player.transform.position;
        yield return new WaitForSeconds(1);
        lastKnownPlayerLocation = player.transform.position;
    }

    private void SearchingState() 
    {
        if (PlayerInFOV())
        {
            agent.speed = chasingSpeed;
            ChangeAlertState(AlertState.Alerted);
            alertLevel = 100;
        }

        agent.SetDestination(lastKnownPlayerLocation);
        if(agent.remainingDistance < 0.5f)
        {
            ChangeAlertState(AlertState.Wandering);
            StartCoroutine(WanderingSearchCoroutine());
        }
    }

    private void WanderingState()
    {
        if (PlayerInFOV())
        {
            agent.speed = chasingSpeed;
            ChangeAlertState(AlertState.Alerted);
            alertLevel = alertLevel = 100;
            StopCoroutine(WanderingSearchCoroutine());
        }
        agent.isStopped = false;
        if (!agent.pathPending && agent.remainingDistance <= 0.5f)
        {
            // If arrived, pick a new random point within the search radius
            Vector3 newTarget = GetRandomPointInRadius(lastKnownPlayerLocation, searchRadius);
            agent.SetDestination(newTarget);
        }

    }
    private Vector3 GetRandomPointInRadius(Vector3 center, float radius)
    {
        // Pick a random direction and distance
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += center;
        NavMeshHit hit;

        // Sample the NavMesh to ensure the point is actually walkable
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        // Fallback to the center if no valid point is found (should be rare)
        return center;
    }

    private IEnumerator WanderingSearchCoroutine()
    {
        yield return new WaitForSeconds(searchDuration);

        if(alertState == AlertState.Wandering)
        {
            ChangeAlertState(AlertState.Returning);
            agent.speed = moveSpeed;
            Debug.Log("returning to patrol");
        }
    }
    private void ReturningState() 
    { 
        if(guardWalkPoints != null)
        {
            agent.SetDestination(guardWalkPoints[currentPatrolDestination].location);
        }
        else
        {
            agent.SetDestination(startPosition);
        }
        
        if (!PlayerInFOV())
        {
            alertLevel -= Time.deltaTime * alertDecay;
        }
        else
        {     
            alertLevel += Time.deltaTime * detectionSpeed * (distanceDrawbackMultiplier / Vector3.Distance(transform.position, player.transform.position));
            if (alertLevel >= 100)
            {
                agent.speed = chasingSpeed;
                ChangeAlertState(AlertState.Alerted);
            }
        }

        if(agent.remainingDistance < 0.5)
        {
            //if(transform.rotation != startRotation)
            //{
            //    transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * rotationSpeed);
            //}
            //else
            //{
                
            //}
            SetNextPatrolDestination();
            ChangeAlertState(AlertState.Patrolling);
        }

    }

    private void SetNextPatrolDestination()
    {
        if(guardWalkPoints != null)
        {
            if(guardWalkPoints.Length > 1)
            {
                currentPatrolDestination++;
                if (currentPatrolDestination >= guardWalkPoints.Length)
                {
                    currentPatrolDestination = 0;
                }
            }
            else if(guardWalkPoints.Length == 1)
            {
                if (transform.rotation.y != guardWalkPoints[0].rotation)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, startRotation, Time.deltaTime * rotationSpeed);
                }
            }
            

        }
    }









    private void PlayerCaught()
    {
        if (!GameManager.Instance.RoundOver)
        {
            GameManager.Instance.PlayerCaught();
            GameManager.Instance.RoundOver = true;

        }
        
    }







    public void ChangeAlertState(AlertState alertStage)
    {
        if(this.alertState != alertStage)
        {
            // state swicthed
            this.alertState = alertStage;
        }     
    }

    private bool PlayerInFOV()
    {
        Collider[] targetsInFOV = Physics.OverlapSphere(transform.position, fovDistance);
        
        foreach (Collider target in targetsInFOV)
        {
            if (target.CompareTag("Player"))
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if(distance < blindSightDistance)
                {
                    return true;
                }
                float signedAngle = Vector3.Angle(transform.forward, target.transform.position - transform.position);
                if (Mathf.Abs(signedAngle) < fovAngle / 2 && Physics.Raycast(transform.position + raycastOffset, target.transform.position - transform.position, out RaycastHit hit, fovDistance, ~ignoreMask))
                {
                    //Debug.Log(hit.collider.name);
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



















    private void OnDrawGizmos()
    {
        Color color = new Color(0, 1, 0, 0.3f);
        if (alertState == AlertState.Suspicious)
        {
            color = Color.Lerp(new Color(0, 1, 0, 0.3f), new Color(1, 0, 0, 0.3f), alertLevel / 100f);
        }
        else if (alertState == AlertState.Alerted)
        {
            color = new Color(1, 0, 0, 0.3f);
        }


        Handles.color = color;
        Handles.DrawSolidArc(transform.position, transform.up, Quaternion.AngleAxis(-fovAngle / 2f, transform.up) * transform.forward, fovAngle, fovDistance);
    }


}
