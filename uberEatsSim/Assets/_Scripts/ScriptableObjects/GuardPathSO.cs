using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New GuardPath", menuName = "Scriptable Objects/GuardPath")]
public class GuardPathSO : ScriptableObject
{
    // THIS SCRIPT IS NO LONGER USED
    [Serializable]
    public struct GuardPath
    {
        public Vector3 location;

        [Range(0, 360)] public float rotation;

        public GuardPath(Vector3 location, float rotation)
        {
            this.location = location;

            this.rotation = rotation;
        }
    }

    public List<GuardPath> GuardRouteLocations = new List<GuardPath>();
}
