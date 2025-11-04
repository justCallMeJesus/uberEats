using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardWalkPoint: MonoBehaviour
{
    [Range(0, 360)] public float rotation;

    public Vector3 location;

    private void Awake()
    {
        location = transform.position;
    }
}
