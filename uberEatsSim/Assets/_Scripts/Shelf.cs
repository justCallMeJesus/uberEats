using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : MonoBehaviour
{
    public ShelfSO shelfSO;

    protected static HashSet<Item> possibleItems = new HashSet<Item>();

    protected static HashSet<Item> spawnedItems = new HashSet<Item>();

    public bool shelfChoosen = false;

    public bool shelfFull = false;



    [SerializeField] protected GameObject[] spawnPoints;

    private void Awake()
    {
        //possibleItems = new HashSet<Item>(shelfSO.items);

       // possibleItems.UnionWith(shelfSO.items);
    }

    public virtual void SpawnItem(Item item)
    {
        
    }

    public virtual void FillEmptyShelf()
    {

    }
}
