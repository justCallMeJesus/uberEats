using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameSaves gameSaves;

    public void PlayGame()

    {
        ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("QUIT!");
        Application.Quit();
    }


    private void ResetGame()
    {
        gameSaves.grandmaAngrinessScale = 100;
        gameSaves.currentRound = 0;
    }
}

