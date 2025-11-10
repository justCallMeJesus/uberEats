using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    public Tutorialroom tutorialroom;
    public bool TutorialComplete = false;
    public GameObject ShoppingList;
    public GameObject Door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ShoppingList.activeSelf == false)
        {
            TutorialComplete = true;
        }
        if (TutorialComplete)
        {
            Door.transform.rotation = Quaternion.Euler(0, 45, 0);


        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You have exited the tutorial room!");
            // Add code here to transition to the next scene or level
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
