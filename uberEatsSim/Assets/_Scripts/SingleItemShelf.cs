using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleItemShelf : Shelf
{
    Item selectedItem;

    private Item ChooseRandomAvailableItem()
    {
        List<Item> availableItems = shelfSO.items.Where(item => !spawnedItems.Contains(item)).ToList();
        if(availableItems.Count > 0)
        {
            int randomIndex = Random.Range(0, availableItems.Count);
            selectedItem = availableItems.ElementAt(randomIndex);
            return selectedItem;
        }
        return null;

        //possibleItems.Remove(selectedItem);

        Debug.Log(selectedItem);
    }

    private void Start()
    {
        //ChooseRandomItem();
        //foreach(var spawnPoint in spawnPoints)
        //{
        //    Instantiate(selectedItem,spawnPoint.transform.position, Quaternion.identity, transform);
        //}
    }

    public override void SpawnItem(Item item)
    {
        selectedItem = item;
        shelfChoosen = true;
        spawnedItems.Add(item);
        shelfFull = true;
        foreach (var spawnPoint in spawnPoints)
        {
            Instantiate(item, spawnPoint.transform.position, Quaternion.identity, transform);
        }
    }

    public override void FillEmptyShelf()
    {
        Debug.Log("Fill empty shelf randomly" +  this.gameObject);
        if (!shelfFull)
        {
            selectedItem = ChooseRandomAvailableItem();
            if(selectedItem != null)
            {
                SpawnItem(selectedItem);
            }
        }
    }
}
