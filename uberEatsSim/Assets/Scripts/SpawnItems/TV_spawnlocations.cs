using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_spawnlocations : MonoBehaviour
{
    public GameObject spawnPos1;
    public GameObject spawnPos2;
    public GameObject spawnPos3;
    public GameObject spawnPos4;
    public GameObject spawnPos5;
    public GameObject spawnPos6;
    public static List<GameObject> allSpawnPos = new List<GameObject>();


    public void SpawnTV()
    {
        allSpawnPos.Add(spawnPos1);
        allSpawnPos.Add(spawnPos2);
        allSpawnPos.Add(spawnPos3);
        allSpawnPos.Add(spawnPos4);
        allSpawnPos.Add(spawnPos5);
        allSpawnPos.Add(spawnPos6);

        int randomIndex = Random.Range(0, allSpawnPos.Count);
        allSpawnPos[randomIndex].SetActive(true);
    }
}
