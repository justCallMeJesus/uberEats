using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialRoomManager : MonoBehaviour
{
    [SerializeField] GameSaves gameSaves;
    [SerializeField] GameObject disownedText;
    [SerializeField] TextMeshProUGUI roundsAmountText;
    // Start is called before the first frame update
    void Start()
    {
        if(gameSaves.grandmaAngrinessScale <= 0)
        {
            roundsAmountText.text = ("You satisfied your Grandmother for " + gameSaves.currentRound + " grocery trips.");
            disownedText.SetActive(true);
            GameInput.instance.playerInputActions.Player.Disable();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
