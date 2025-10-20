using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPlayerDetector : MonoBehaviour
{
    [SerializeField] private Item parentItem;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteraction player))
        {
            Debug.Log("Player entered interaction zone");
            if (CheckItemList())
            {
                player.interactableItems.Add(parentItem);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInteraction player))
        {
            if (CheckItemList())
            {
                player.interactableItems.Remove(parentItem);
            }
            
        }
    }

    public bool CheckItemList()
    {
        bool matchFound = GameManager.Instance.selectedItems.Any(item => item.itemSO == parentItem.itemSO);
        return matchFound;
    }
}
