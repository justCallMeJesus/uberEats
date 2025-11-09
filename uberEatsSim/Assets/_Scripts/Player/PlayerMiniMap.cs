using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMiniMap : MonoBehaviour
{
    public GameObject target;
    void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        
        transform.position = target.transform.position + new Vector3(0, 150, 0);
    }
}
