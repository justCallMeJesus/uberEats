using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class master_item_spawn : MonoBehaviour
{
    public TV_spawnlocations tv_spawnlocations;

    void Start()
    {
        

        int randomIndex = Random.Range(0, 6);
        int randomIndex2 = Random.Range(0, 6);
        
        while (randomIndex2 == randomIndex){
            randomIndex2 = Random.Range(0, 6);
        }
        int randomIndex3 = Random.Range(0, 6);
        while (randomIndex3 == randomIndex || randomIndex3 == randomIndex2){
            randomIndex3 = Random.Range(0, 6);
        }

        if (randomIndex == 0 || randomIndex2 == 0 || randomIndex3 == 0){
            tv_spawnlocations.spawnPos1.SetActive(true);
        }
        
    }
}
