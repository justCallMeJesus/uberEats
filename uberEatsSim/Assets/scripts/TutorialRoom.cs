using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorialroom : MonoBehaviour
{

    public bool WKey = false;
    public bool SKey = false;
    public bool AKey = false;
    public bool DKey = false;
    public bool Movement = false;   

    public GameObject DoorDoor;
    public bool TutorialComplete = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            WKey = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            SKey = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            AKey = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            DKey = true;

        }
            
        if (WKey && SKey && AKey && DKey)
        {
            Movement = true;
            
                


        }
        

    }
    

}

