using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static ItemSO;

public class GameManager : MonoBehaviour
{


    public List<SelectedItems> selectedItems = new List<SelectedItems>();

    public static GameManager Instance;

    public event Action<SelectedItems> OnItemTypeCollected;

    [Serializable]
    public struct SelectedItems
    {
        // The type of item (replace 'MyItemType' with your actual item class or type)
        public Item item;

        // The integer value associated with this specific item
        public int count;
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    public void ReduceItemCount(Item itemToReduce)
    {
        // 1. Find the index of the ItemData entry that holds the matching item
        int index = selectedItems.FindIndex(data => data.item.itemSO == itemToReduce.itemSO);

        // 2. Check if the item was found
        if (index != -1)
        {
            Debug.Log("index found");
            // 3. Retrieve the struct from the list (this creates a *copy*)
            SelectedItems dataToModify = selectedItems[index];

            // 4. Check if we have any left before reducing
            if (dataToModify.count > 0)
            {
                // 5. Modify the 'count' property of the *copy*
                dataToModify.count--;

                // 6. Replace the old struct in the list with the modified copy
                selectedItems[index] = dataToModify;

                Debug.Log($"Used one {itemToReduce.name}. Remaining: {dataToModify.count}");

                // 7. Check for removal if count hits zero
                if (dataToModify.count <= 0)
                {
                    RemoveItemFromList(index);
                }
            }
            else
            {
                Debug.Log($"Cannot reduce {itemToReduce.name}. Count is already zero.");
            }
        }
        else
        {
            Debug.Log($"Item {itemToReduce.name} not found in inventory.");
        }
    }

    private void RemoveItemFromList(int index)
    {
        // Use RemoveAt since we already have the index
        OnItemTypeCollected?.Invoke(selectedItems[index]);
        selectedItems.RemoveAt(index);
        Debug.Log("Item count reached zero and was removed from the list.");
    }

    public bool ItemInList(Item item)
    {
        int index = selectedItems.FindIndex(data => data.item.itemSO == item.itemSO);
        if(index == -1)
        {
            return false;
        }
        return true;
    }
}
