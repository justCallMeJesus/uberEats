using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Collection Set")]
public class SelectableItemsetsSO : ScriptableObject
{
    //public ShelfSO[] itemSets;

    [Serializable]
    public struct ItemSet
    {
        public ShelfSO shelfSO;

        public int amount;

        public int minimumPerItem;

        public int maximumPerItem;

        public ItemSet(ShelfSO shelfSO, int amount, int minimumPerItem, int maximumPerItem)
        {
            this.shelfSO = shelfSO;
            this.amount = amount;
            this.minimumPerItem = minimumPerItem;
            this.maximumPerItem = maximumPerItem;
        }
    }

    public List<ItemSet> itemSets = new List<ItemSet>();
}
