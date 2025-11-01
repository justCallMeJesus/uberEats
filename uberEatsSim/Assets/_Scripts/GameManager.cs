using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static ItemSO;

public class GameManager : MonoBehaviour
{

    private List<SelectedItems> originalSelectedItems = new List<SelectedItems>();
    public List<SelectedItems> selectedItems = new List<SelectedItems>();

  

    public static GameManager Instance;

    public event Action<SelectedItems> OnItemTypeCollected;

    public float timeElapsed;
    private float givenTime;

    [Header("Time Settings")]
    [SerializeField] private float standardStartTime = 30f;
    [SerializeField] private float addonTimePerItem = 5f;
    [SerializeField] private float timePenaltyNormalizer = 3f;


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

    public void ItemsSelected()
    {
        originalSelectedItems = selectedItems;
        givenTime = standardStartTime + addonTimePerItem * selectedItems.Count;
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

    public int GetMissedPriority()
    {
        int missedPririty = 0;
        foreach(SelectedItems item in selectedItems)
        {
            missedPririty += item.item.itemSO.priority * item.count;
        }

        return missedPririty;
    }

    private int CalculateTimePenalty()
    {
        int penalty = 0;
        float timeOverdrawn = 0;
        timeOverdrawn = givenTime - timeElapsed;
        if( timeOverdrawn > 0)
        {
            return 0;
        }
        penalty = Mathf.Abs(Mathf.RoundToInt(timeOverdrawn / timePenaltyNormalizer));
        return penalty;
        
    }

    private int CalculateMissedItemsPenalty()
    {
        int penalty = 0;
        foreach (SelectedItems item in selectedItems)
        {
            penalty += item.item.itemSO.priority * item.count;
        }
        return penalty;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log(CalculateTimePenalty());
            PlayerCaught();
        }

        timeElapsed = Time.time;
        
    }

    public void PlayerCaught()
    {
        int previousGrandmaAngriness = 80;
        int timePenalty = CalculateTimePenalty();
        int missedItemPenalty = CalculateMissedItemsPenalty();
        int newGrandmaAngriness = previousGrandmaAngriness - timePenalty - missedItemPenalty;
        if(newGrandmaAngriness < 0)
        {
            newGrandmaAngriness = 0;

            // game over
        }
        // disable player controls
        GameInput.instance.DisableControls();
        // fade in player caught screen
        UIManager.Instance.ShowCaughtScreen();

        StartCoroutine(BlendInInfoScreen(previousGrandmaAngriness, timePenalty, missedItemPenalty, newGrandmaAngriness));
        // update angryness scale
        // blend in next button
        // load next scene
    }

    private IEnumerator BlendInInfoScreen(int prevAngriness, int timePenalty, int missedItemPenalty, int newAngriness)
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowPreviousAngrinessScale(prevAngriness);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowTimePenalty(timePenalty);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowMissedItemPenalty(missedItemPenalty);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowNewAngrinessScale(newAngriness);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowGoHome();


    }
}
