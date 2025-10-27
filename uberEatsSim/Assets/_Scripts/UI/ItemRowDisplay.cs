using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemRowDisplay : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI requiredCountText;

    private Item displayedItem = null;

    /// <summary>
    /// Sets the text for the item name and the required count upon creation.
    /// </summary>
    /// 
    private void Start()
    {
        GameManager.Instance.OnItemTypeCollected += Instance_OnItemTypeCollected;
    }

    private void Instance_OnItemTypeCollected(GameManager.SelectedItems itemType)
    {
        if(displayedItem.itemSO == itemType.item.itemSO)
        {
            CrossOutItem();
        }
    }

    public void Initialize(Item itemType, int requiredCount)
    {
        displayedItem = itemType;
        // Set the item name text
        if(requiredCount > 1)
        {
            itemNameText.text = $"{itemType.itemSO.multiplesName}";
        }
        else
        {
            itemNameText.text = $"{itemType.itemSO.name}";
        }
        

        // Format the required count text (e.g., "x5" or just "5")
        requiredCountText.text = $"{requiredCount}";
    }

    private void CrossOutItem()
    {
        itemNameText.text = $"<s>{itemNameText.text}</s>";
    }
}
