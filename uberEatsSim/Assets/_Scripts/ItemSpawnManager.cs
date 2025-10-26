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

    [SerializeField] private ItemCollectorUI itemCollectorUI;

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
            // foreach category in the collection of category
            for (int i = 0; i < itemSet.amount; i++)
            {
                // for the amount if needed items of that category
                // add all items that are not yet in the selectedItems
                List<Item> availableItems = itemSet.shelfSO.items.Where(item => !selectedItems.Contains(item)).ToList();

                // select random item from availableItems
                Item selectedItem = availableItems[Random.Range(0, availableItems.Count)];

                // add that item to list
                selectedItems.Add(selectedItem);
                
                int amount = Random.Range(itemSet.minimumPerItem, itemSet.maximumPerItem + 1);
                GameManager.Instance.selectedItems.Add(new GameManager.SelectedItems { item = selectedItem, count = amount });

                ItemCollectorUI.ItemData newItem = new ItemCollectorUI.ItemData
                {
                    itemName = selectedItem,
                    requiredCount = amount,
        
                };
                itemCollectorUI.AddItemRequirement(newItem);
            }
        }
        //GameManager.Instance.selectedItems = this.selectedItems;
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
