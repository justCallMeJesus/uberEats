using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Guard Saver", menuName = "Scriptable Objects/Guard Saver")]
public class GameSaves : ScriptableObject
{
    public int grandmaAngrinessScale = 100;
    public HashSet<GuardAI> extraSpawnedGuards = new HashSet<GuardAI>();
}
