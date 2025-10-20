using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Shelf Collection", menuName = "Scriptable Objects/Shelf Collection")]
public class ShelfSO : ScriptableObject
{
    public List<Item> items = new List<Item>();    
}
