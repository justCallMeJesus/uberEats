using UnityEngine;

public class Tutorialroom : MonoBehaviour
{
    public bool WKey = false;
    public bool SKey = false;   
    public bool AKey = false;   
    public bool DKey = false;   
    public GameObject MovementCheck;
    public GameObject ScooterCheck;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        MovementCheck.SetActive(true);
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
        // if on scooter or someshi idfk then scooterchek.SetActive(true);
        if (WKey && SKey && AKey && DKey)
        {
            MovementCheck.SetActive(false);

        }
    }
}

