using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectorUI : MonoBehaviour
{
    [SerializeField] private GameObject shoppingList;
    [SerializeField] private GameInput gameInput;

    public GameObject itemRowPrefab;

    private HashSet<Item> displayedItems = new HashSet<Item>();





    public class ItemData
    {
        public Item itemName;
        public int requiredCount;
    }


    public void AddItemRequirement(ItemData data)
    {
        // Check if a row for this item already exists in the set.
        if (displayedItems.Contains(data.itemName))
        {
            // The item is already on the list, so do nothing.
            return;
        }

        // Item does not exist, so create a NEW row.

        // Instantiate the prefab as a child of this container
        GameObject newRow = Instantiate(itemRowPrefab, transform);
        newRow.name = data.itemName + " Row";

        // Initialize the display script on the new row
        newRow.GetComponent<ItemRowDisplay>().Initialize(data.itemName, data.requiredCount);

        // Add the item to our set so we don't duplicate it.
        displayedItems.Add(data.itemName);
    }

    
}
