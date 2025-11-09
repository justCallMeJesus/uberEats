using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static GuardPathSO;

public class GuardManager : MonoBehaviour
{
    [SerializeField] private GuardAI[] defaultGuards;
    [SerializeField] private List<GuardAI> restGuards;

    [SerializeField] private GameSaves guardSavesSO;

    public int currentExtraGuardAmount = 0;

    public static GuardManager Instance;

    private void Awake()
    {
        if(Instance == null) { Instance = this; } else { Destroy(Instance); }
    }

    private void Start()
    {
        foreach (var guard in defaultGuards)
        {
            Instantiate(guard, guard.spawnPoint, Quaternion.AngleAxis(guard.guardWalkPoints[0].rotation, Vector3.up));
        }

        foreach (var guard in guardSavesSO.extraSpawnedGuards)
        {
            restGuards.Remove(guard);
            Instantiate(guard, guard.spawnPoint, Quaternion.AngleAxis(guard.guardWalkPoints[0].rotation, Vector3.up));
        }

    }

    public void ChooseExtraGuards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GuardAI newGuard = restGuards[Random.Range(0, restGuards.Count)];
            restGuards.Remove(newGuard);
            guardSavesSO.extraSpawnedGuards.Add(newGuard);
        }
    }
}
