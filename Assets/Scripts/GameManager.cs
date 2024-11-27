using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Static reference for GameManager, so I don't have to find it every time
    [SerializeField] string sceneToLoad; // name of the scene to load after player collected all coins

    List<Coin> coins = new List<Coin>();  // list of all coins in the scene
    
    int coinsCollected = 0; // coins counter
    public int CoinsCollected => coinsCollected; // public getter
    public static event Action<Coin> OnCoinCollected; // event to trigger once a coin is collected

    bool isPaused;
    public bool IsPaused => isPaused;
    public static event Action<bool> OnGamePaused;  // event to trigger on game pause/unpause

    
    public static event Action<TimeSpan> OnTimePlayedChanged; // event for the timer
    Stopwatch stopwatch;  // a stopwatch to count the game time
    public TimeSpan TimePlayed => stopwatch.Elapsed; // The current game time span


    void Awake()
    {
        // Set GameManager Instance reference
        Instance = this;

        // Get all coins in the scene
        coins = FindObjectsOfType<Coin>().ToList();

        // initialize and start the clock
        stopwatch = new Stopwatch();
        stopwatch.Start();
    }
    
    void Update()
    {
        CheckPause();
        if(stopwatch.IsRunning)
        {
            OnTimePlayedChanged?.Invoke(stopwatch.Elapsed);
        }
    }

    // Adding coins to list
    public void AddCoinToList(Coin coin)
    {
        if (coin == null || coins.Contains(coin)) // check if coin exists and is not already in the list
            return;
        coins.Add(coin); // add to the coin list
    }

    public void CollectCoin(Coin coinCollected)
    {
        if (!coinCollected) // return if there is no coin
            return;

        coinsCollected++; // add 1 to the counter
        coins.Remove(coinCollected); // remove from the list

        OnCoinCollected?.Invoke(coinCollected); // call event to notify other scripts
        
        if (coins.Count <= 0) // check is list is empty, if true end the game
        {
            EndGame();
        }
    }

    void EndGame()
    {
        // stop the clock
        stopwatch.Stop();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(sceneToLoad); // load new scene
    }

    void CheckPause()
    {
        if(Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused) 
               UnpauseGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0.0f; // pause time
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        stopwatch.Stop();
        OnGamePaused?.Invoke(true); // call event to notify the game state changed
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f; // continue time
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        stopwatch.Start();
        OnGamePaused?.Invoke(false); // call event to notify the game state changed
    }
    
}
