using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static ItemSO;

public class GameManager : MonoBehaviour
{

    private List<SelectedItems> originalSelectedItems = new List<SelectedItems>();
    public List<SelectedItems> selectedItems = new List<SelectedItems>();
    public List<SelectedItems> collectedItems = new List<SelectedItems>();
    public List<SelectedItems> paidItems = new List<SelectedItems>();

    public bool RoundOver = false;
  

    public static GameManager Instance;

    public event Action<SelectedItems> OnItemTypeCollected;

    public float timeElapsed;
    private float givenTime;
    private float timeSinceStart;

    [Header("Time Settings")]
    [SerializeField] private float standardStartTime = 30f;
    [SerializeField] private float addonTimePerItem = 7f;
    [SerializeField] private float timePenaltyNormalizer = 1.5f;

    [Header("Other Managers")]
    [SerializeField] private GuardManager guardManager;
    [SerializeField] private GameSaves gameSave;


    [Serializable]
    public struct SelectedItems
    {
        // The type of item (replace 'MyItemType' with your actual item class or type)
        public ItemSO itemSO;

        // The integer value associated with this specific item
        public int count;
    }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        timeSinceStart = Time.time;
    }

    private void Start()
    {
        originalSelectedItems = selectedItems;
    }

    public void ReduceItemCount(ItemSO itemToReduce)
    {
        // 1. Find the index of the ItemData entry that holds the matching item
        int index = selectedItems.FindIndex(data => data.itemSO == itemToReduce);

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

                AddItemToInventory(itemToReduce);

                // 6. Replace the old struct in the list with the modified copy
                selectedItems[index] = dataToModify;

                Debug.Log($"Used one {itemToReduce.name}. Remaining: {dataToModify.count}");

                // 7. Check for removal if count hits zero
                if (dataToModify.count <= 0)
                {
                    RemoveItemFromList(selectedItems, index);
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

    public void ReduceItemFromCollectedItems(ItemSO itemToReduce)
    {
        int index = collectedItems.FindIndex(data => data.itemSO == itemToReduce);

        // 2. Check if the item was found
        if (index != -1)
        {
            Debug.Log("index found");
            // 3. Retrieve the struct from the list (this creates a *copy*)
            SelectedItems dataToModify = collectedItems[index];

            // 4. Check if we have any left before reducing
            if (dataToModify.count > 0)
            {
                // 5. Modify the 'count' property of the *copy*
                dataToModify.count--;

                AddItemToInventory(itemToReduce);

                // 6. Replace the old struct in the list with the modified copy
                collectedItems[index] = dataToModify;

                Debug.Log($"Used one {itemToReduce.name}. Remaining: {dataToModify.count}");

                // 7. Check for removal if count hits zero
                if (dataToModify.count <= 0)
                {
                    collectedItems.RemoveAt(index);
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

    private void AddItemToInventory(ItemSO itemToAdd)
    {
        //ReduceItemFromCollectedItems(itemToAdd);
        int index = collectedItems.FindIndex(data => data.itemSO == itemToAdd);
        
        if (index != -1)
        {
            SelectedItems dataToModify = collectedItems[index];
            dataToModify.count++;

            collectedItems[index] = dataToModify;
        }
        else
        {
            collectedItems.Add(new SelectedItems { itemSO = itemToAdd, count = 1 });
        }
    }

    public void AddItemToPaid(ItemSO itemToAdd)
    {
        ReduceItemFromCollectedItems(itemToAdd);
        int index = paidItems.FindIndex(data => data.itemSO == itemToAdd);

        if (index != -1)
        {
            SelectedItems dataToModify = paidItems[index];
            dataToModify.count++;

            paidItems[index] = dataToModify;
        }
        else
        {
            paidItems.Add(new SelectedItems { itemSO = itemToAdd, count = 1 });
        }
    }

    public void ItemsSelected()
    {
        originalSelectedItems = selectedItems;
        givenTime = standardStartTime + addonTimePerItem * selectedItems.Count;
        UIManager.Instance.SetTimeGiven(Mathf.RoundToInt(givenTime));
    }

    private void RemoveItemFromList(List<SelectedItems> list, int index)
    {
        // Use RemoveAt since we already have the index
        OnItemTypeCollected?.Invoke(list[index]);
        list.RemoveAt(index);
        Debug.Log("Item count reached zero and was removed from the list.");
    }

    public bool ItemInList(Item item)
    {
        int index = selectedItems.FindIndex(data => data.itemSO == item.itemSO);
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
            missedPririty += item.itemSO.priority * item.count;
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
    private int CalculateCaughtMissedItemsPenalty()
    {
        int penalty = 0;
        foreach (SelectedItems item in selectedItems)
        {
            penalty += item.count;
        }
        return penalty;
    }

    private int CalculateMissedItemsPenalty(List<SelectedItems> itemList)
    {
        int penalty = 0;
        foreach (SelectedItems item in itemList)
        {
            penalty += item.count;
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
        if (Input.GetKeyDown(KeyCode.P))
        {
            guardManager.ChooseExtraGuards(1);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        timeElapsed = Time.time - timeSinceStart;
        UIManager.Instance.SetTimePassed(Mathf.Round(timeElapsed * 10f) / 10f);
        
    }

    public void PlayerCaught()
    {
        if (RoundOver)
        {
            return;
        }
        RoundOver = true;
        gameSave.currentRound++;
        GuardManager.Instance.ChooseExtraGuards(2);

        int previousGrandmaAngriness = gameSave.grandmaAngrinessScale;
        int timePenalty = CalculateTimePenalty();
        int missedItemPenalty = CalculateCaughtMissedItemsPenalty();
        int caughtPenalty = 5;
        int newGrandmaAngriness = previousGrandmaAngriness - timePenalty - missedItemPenalty - caughtPenalty;
        if(newGrandmaAngriness < 0)
        {
            newGrandmaAngriness = 0;

            // game over
        }

        gameSave.grandmaAngrinessScale = newGrandmaAngriness;
        UIManager.Instance.SetExitText("You got caught!");
        // disable player controls
        GameInput.instance.DisableControls();
        // fade in player caught screen
        UIManager.Instance.ShowCaughtScreen();

        StartCoroutine(BlendInInfoScreen(previousGrandmaAngriness, timePenalty, missedItemPenalty, caughtPenalty, newGrandmaAngriness));
        // update angryness scale
        // blend in next button
        // load next scene
    }

    public void PlayerLeft()
    {
        if (RoundOver)
        {
            return;
        }
        RoundOver = true;
        gameSave.currentRound++;
        if(collectedItems.Count > 0)
        {
            GuardManager.Instance.ChooseExtraGuards(4);
        }
        else
        {
            GuardManager.Instance.ChooseExtraGuards(2);
        }
        

        int previousGrandmaAngriness = gameSave.grandmaAngrinessScale;
        int timePenalty = CalculateTimePenalty();
        int missedItemPenalty = CalculateMissedItemsPenalty(selectedItems);
        int caughtPenalty = 0;
        int newGrandmaAngriness = previousGrandmaAngriness - timePenalty - missedItemPenalty - caughtPenalty;
        if (newGrandmaAngriness < 0)
        {
            newGrandmaAngriness = 0;

            // game over
        }
        gameSave.grandmaAngrinessScale = newGrandmaAngriness;
        UIManager.Instance.SetExitText("You left the Store!");
        // disable player controls
        GameInput.instance.DisableControls();
        // fade in player caught screen
        UIManager.Instance.ShowCaughtScreen();

        StartCoroutine(BlendInInfoScreen(previousGrandmaAngriness, timePenalty, missedItemPenalty, caughtPenalty, newGrandmaAngriness));
    }

    private IEnumerator BlendInInfoScreen(int prevAngriness, int timePenalty, int missedItemPenalty, int caughtPenalty, int newAngriness)
    {
        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowPreviousAngrinessScale(prevAngriness);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowTimePenalty(timePenalty);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowMissedItemPenalty(missedItemPenalty);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowCaughtPenalty(caughtPenalty);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowNewAngrinessScale(newAngriness);

        yield return new WaitForSeconds(1f);
        UIManager.Instance.ShowGoHome();


    }

    public void ResetGame()
    {
        gameSave.grandmaAngrinessScale = 100;
        gameSave.extraSpawnedGuards.Clear();
    }
}
