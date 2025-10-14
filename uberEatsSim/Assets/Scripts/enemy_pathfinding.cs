using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_pathfinding : MonoBehaviour
{
    public NavMeshAgent agent;
    public int randomPosX;
    public int randomPosZ; 
    public Vector3 randomPosVector;
    public Vector3 velocity;

    
    private void Start()
    {
        
        randomPosVector = getRandomPos();
        agent.SetDestination(randomPosVector);
    }
    void Update()
    {
        velocity = agent.velocity;
        if (velocity == Vector3.zero)
        {
            
            agent.SetDestination(getRandomPos());
     
        }

    }
    Vector3 getRandomPos()
    {
        randomPosX = Random.Range(0, 20);
        randomPosZ = Random.Range(0, 20);
        randomPosVector = new Vector3(randomPosX, 0, randomPosZ);
        return randomPosVector;
    }
}
