using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    [Header("Shopping List Fields")]
    [SerializeField] private GameObject shoppingList;

    [Header("Caught Screen Fields")]
    [SerializeField] private GameObject caughtScreen;

    [Header("End Screen Info Fields")]
    [SerializeField] private GameObject prevAngrinessScale;
    [SerializeField] private Slider prevAngrinessScaleBar;
    [SerializeField] private TextMeshProUGUI timePenaltyInfo;
    [SerializeField] private TextMeshProUGUI missedItemPenaltyInfo;
    [SerializeField] private TextMeshProUGUI caughtPenaltyInfo;
    [SerializeField] private GameObject newAngrinessScale;
    [SerializeField] private Slider newAngrinessScaleBar;
    [SerializeField] private GameObject goHomeButton;

    [Header("Timers")]
    [SerializeField] private TextMeshProUGUI timePassed;



    public static UIManager Instance { get; private set; }


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnShoppingListPressed += GameInput_OnShoppingListPressed;
        gameInput.OnShoppingListReleased += GameInput_OnShoppingListReleased;

        HideShoppingList();
    }

    private void OnDestroy()
    {
        gameInput.OnShoppingListPressed -= GameInput_OnShoppingListPressed;
        gameInput.OnShoppingListReleased -= GameInput_OnShoppingListReleased;
    }

    private void GameInput_OnShoppingListReleased()
    {
        HideShoppingList();
    }

    private void GameInput_OnShoppingListPressed()
    {
        ShowShoppingList();
    }

    public void ShowShoppingList()
    {
        shoppingList.SetActive(true);
    }

    public void HideShoppingList()
    {
        shoppingList.SetActive(false);
    }

    public void ShowCaughtScreen()
    {
        caughtScreen.SetActive(true);
    }

    public void ShowPreviousAngrinessScale(float angriness)
    {
        prevAngrinessScaleBar.value = angriness;
        prevAngrinessScale.SetActive(true);
    }

    public void ShowTimePenalty(int  timePenalty)
    {
        timePenaltyInfo.text = timePenalty.ToString();
        timePenaltyInfo.gameObject.SetActive(true);
    }

    public void ShowMissedItemPenalty(int missedItemPenalty)
    {
        missedItemPenaltyInfo.text = missedItemPenalty.ToString();
        missedItemPenaltyInfo.gameObject.SetActive(true);
    }

    public void ShowCaughtPenalty(int caughtPenalty)
    {
        caughtPenaltyInfo.text = caughtPenalty.ToString();
        caughtPenaltyInfo.gameObject.SetActive(true);
    }

    public void ShowNewAngrinessScale(int angriness)
    {
        newAngrinessScaleBar.value = angriness;
        newAngrinessScale.SetActive (true);
    }

    public void ShowGoHome()
    {
        goHomeButton.SetActive(true);
    }

    public void UpdateTime(float time)
    {

    }
}
