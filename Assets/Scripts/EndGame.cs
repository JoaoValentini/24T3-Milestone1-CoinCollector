using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI victoryText;
    
    void Awake()
    {
        // Set the victory text to the coins collected and time spent, with correct formatting
        victoryText.text = $"You Won!! \n " +
            $"Coins Collected: { GameManager.Instance.CoinsCollected} \n" +
            $"Time: {GameManager.Instance.TimePlayed.ToString(@"mm\:ss\.fff")}";
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
