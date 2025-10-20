using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemSpawnManager : MonoBehaviour
{
    public Shelf[] shelves;
    public List<Shelf> neededShelves = new List<Shelf>();

    public SelectableItemsetsSO collection;

    public List<Item> selectedItems = new List<Item>();

    private void Start()
    {
        shelves = FindObjectsOfType<Shelf>();
        foreach (Shelf shelf in shelves)
        {
            foreach(var itemSet in collection.itemSets)
            {
                if(shelf.shelfSO == itemSet.shelfSO)
                {
                    neededShelves.Add(shelf);
                }
            }
        }

        SelectItemsFromCollection();
        SpawnSelectedItems();
        FillEmptyShelves();
    }

    private void SelectItemsFromCollection()
    {
        foreach (var itemSet in collection.itemSets)
        {
            for (int i = 0; i < itemSet.amount; i++)
            {
                List<Item> availableItems = itemSet.shelfSO.items.Where(item => !selectedItems.Contains(item)).ToList();
                selectedItems.Add(availableItems[Random.Range(0, availableItems.Count)]);               
            }
        }
        GameManager.Instance.selectedItems = this.selectedItems;
    }

    private void SpawnSelectedItems()
    {
        foreach(Item item in selectedItems)
        {
            List<Shelf> itemSpecificShelfs = FindObjectsOfType<Shelf>().Where(shelf => shelf.shelfSO.items.Contains(item) && shelf.shelfChoosen == false).ToList();
            itemSpecificShelfs[Random.Range(0, itemSpecificShelfs.Count)].SpawnItem(item);
        }
    }

    private void FillEmptyShelves()
    {
        List<Shelf> emptyShelves = FindObjectsOfType<Shelf>().Where(shelf => shelf.shelfFull == false).ToList();
        foreach(Shelf shelf in emptyShelves)
        {
            shelf.FillEmptyShelf();
        }
    }

    
}
