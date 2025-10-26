using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameInput gameInput;

    [Header("Shopping List Fields")]
    [SerializeField] private GameObject shoppingList;


    // Start is called before the first frame update
    void Start()
    {
        gameInput.OnShoppingListPressed += GameInput_OnShoppingListPressed;
        gameInput.OnShoppingListReleased += GameInput_OnShoppingListReleased;
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
}
