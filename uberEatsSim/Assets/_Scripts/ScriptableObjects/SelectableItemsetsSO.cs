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

        public ItemSet(ShelfSO shelfSO, int amount)
        {
            this.shelfSO = shelfSO;
            this.amount = amount;
        }
    }

    public List<ItemSet> itemSets = new List<ItemSet>();
}
