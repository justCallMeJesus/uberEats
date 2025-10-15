using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{

    public List <Shelf> possibleShelves = new List <Shelf> ();

    private List<Shelf> selectedShelves = new List<Shelf>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        int randomIndex = Random.Range(0, possibleShelves.Count);

        Shelf randomItem = possibleShelves[randomIndex];

        possibleShelves.Remove(randomItem);

        selectedShelves.Add(randomItem);
    }
}
