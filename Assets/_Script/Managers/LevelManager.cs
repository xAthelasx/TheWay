using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { INITIAL, GAME, PAUSE, GAMEOVER}
public class LevelManager : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] GameObject camera1, camera2;
    
    [SerializeField] float timeLevel;

    [SerializeField] TMP_Text keyText;
    [SerializeField] TMP_Text secretText;
    [SerializeField] TMP_Text timerText;
    #endregion

    #region Private Variables
    GameState gameState;
    bool asKey;
    float secretLevel;
    float actualSecret;
    float actualTime;
    #endregion

    #region Public Variables
    public GameState GameState { get { return gameState; } }
    #endregion

    #region MonoBehaviour Method
    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if(gameState == GameState.GAME) { CountDown(); }
    }
    #endregion

    #region Private Method
    private void RefreshUI()
    {
        keyText.text = asKey ? "Key 1 / 1" : "Key 0 / 1";
        secretText.text = $"Secret: {actualSecret} / {secretLevel}";
    }

    private void CountDown()
    {
        actualTime -= Time.deltaTime;
        if (actualTime <= 0)
        {
            ChangeGameState(GameState.GAMEOVER);
        }
        else
        {
            string minutes = (int)actualTime / 60 < 10 ? $"0{(int)actualTime / 60}" : $"{(int)actualTime / 60}";
            string seconds = (int)actualTime % 60 < 10 ? $"0{(int)actualTime % 60}" : $"{(int)actualTime % 60}";
            
            timerText.text = $"Time Left: {minutes}:{seconds}";
        }
    }
    #endregion

    #region Public Method
    public void ChangeGameState(GameState newState)
    {
        gameState = newState;
    }

    public void ItemFind(string tag)
    {
        switch (tag)
        {
            case "Key":
                asKey = !asKey;
                break;
            case "Secret":
                actualSecret++;
                break;
        }
        RefreshUI();
    }
    #endregion

    #region Corroutines
    IEnumerator StartGame()
    {
        ChangeGameState(GameState.INITIAL);
        secretLevel = GameObject.FindGameObjectsWithTag("Secret").Length;
        asKey = false;
        actualSecret = 0;
        actualTime = timeLevel;
        RefreshUI();
        yield return new WaitForSeconds(2f);
        camera2.SetActive(false);
        camera1.SetActive(true);
        yield return new WaitForSeconds(2f);
        ChangeGameState(GameState.GAME);
    }
    #endregion
}
