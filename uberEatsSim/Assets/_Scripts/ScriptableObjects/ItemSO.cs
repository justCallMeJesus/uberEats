using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        Fruit,
        Alcohol,
        Tool
    }

    public new string name;

    public string multiplesName;

    public float value;

    public ItemType type;

    public float pickupTime;
    
}
