using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Item> items = new List<Item>(); 

    private List<Item> selectedItems = new List<Item>();

    private int amountToSelect = 3;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < amountToSelect; i++)
        {
            int randomIndex = Random.Range(0, items.Count);

            // 2. Access the element at that random index.
            Item randomItem = items[randomIndex];

            items.Remove(randomItem);

            selectedItems.Add(randomItem);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
