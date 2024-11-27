using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public RectTransform coinsPanelRect;
    public RectTransform coinsIcon;
    [SerializeField] float coinIconRotateSpeed = 90;
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject pausePanel;
    
    void Start()
    {
        //Subscribe to CoinCollected event
        GameManager.OnCoinCollected += OnCoinCollected;
        //Subscribe to the time event
        GameManager.OnTimePlayedChanged += OnPlayedTimeChanged;
        //Subscribe to Pause event
        GameManager.OnGamePaused += OnGamePaused;

        // Disable pause panel on start in case it is active
        pausePanel.SetActive(false);
    }

    void Update()
    {
        coinsIcon.Rotate(0, coinIconRotateSpeed * Time.deltaTime, 0);
    }

    private void OnDestroy()
    {
        //Unsubscribe to CoinCollected event
        GameManager.OnCoinCollected -= OnCoinCollected;
        //Subscribe to the time event
        GameManager.OnTimePlayedChanged -= OnPlayedTimeChanged;
        //Unsubscribe to Pause event
        GameManager.OnGamePaused -= OnGamePaused;
    }

    private void OnPlayedTimeChanged(TimeSpan span)
    {
        // formatting TimeSpan to string
        // https://learn.microsoft.com/en-us/dotnet/standard/base-types/custom-timespan-format-strings#mSpecifier
        
        timeText.text = span.ToString(@"mm\:ss\.fff");
    }

    private void OnGamePaused(bool isPaused)
    {
        pausePanel.SetActive(isPaused); // enable / disable pause panel based on game state
    }

    void OnCoinCollected(Coin coin)
    {
        // Update text
        coinsText.text = GameManager.Instance.CoinsCollected.ToString();
        StopAllCoroutines(); // stop previous animations, if any
        StartCoroutine(CollectCoinRoutine()); // start coin ui animation
    }

    // coroutines doc https://docs.unity3d.com/ScriptReference/Coroutine.html
    // A coroutine is a function that can suspend its execution (yield) until the given Yield Instruction finishes.
    IEnumerator CollectCoinRoutine()
    {
        float scale = 1;
        float time = 0; // current animation time
        float scaleTime = 0.05f; // time for scale to reach max scale
        float maxScale = 1.35f;

        coinsPanelRect.localScale = Vector3.one;

        while (time <= scaleTime) // increase size
        {
            time += Time.deltaTime;
            scale = Mathf.Lerp(1,maxScale, time / scaleTime);
            coinsPanelRect.localScale = new Vector3(scale, scale, scale);
            yield return null; // use this to wait until next frame
        }

        time = 0;
        float returnTime = 0.15f;
        while (time <= returnTime) // return to normal size
        {
            time += Time.deltaTime;
            scale = Mathf.Lerp(maxScale, 1, time / returnTime);
            coinsPanelRect.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }

    public void OnClickContinue()
    {
        GameManager.Instance.UnpauseGame();
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f; // continue time
    }
}
