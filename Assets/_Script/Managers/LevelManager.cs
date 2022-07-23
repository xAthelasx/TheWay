using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum GameState { INITIAL, GAME, PAUSE, GAMEOVER} //Enumerado con los momentos de juego.
public class LevelManager : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] GameObject camera1, camera2; //Variables con las cámaras.
    [SerializeField] GameObject exit; //Variable para la salida.
    
    [SerializeField] float timeLevel; //Variable que nos indicará el tiempo del nivel.

    [SerializeField] TMP_Text keyText; //Texto de conteo de llave.
    [SerializeField] TMP_Text secretText; //Texto de conteo de Secretos.
    [SerializeField] TMP_Text timerText; //Texto de tiempo.
    #endregion

    #region Private Variables
    GameState gameState; //Variable de estado de juego.
    bool asKey; //Booleano que nos indica si tenemos la llave.
    float secretLevel; //Booleano que nos cuenta el número de secretos que hay en el nivel.
    float actualSecret; //Número de secrertos encontrados.
    float actualTime; //Tiempo actual.
    #endregion

    #region Public Variables
    public GameState GameState { get { return gameState; } } //Variable que nos devuelve el estado del juego.
    #endregion

    #region MonoBehaviour Method
    private void Start()
    {
        StartCoroutine(StartGame()); //Iniciamos la corrutina de empezar el juego.
    }

    private void Update()
    {
        if(gameState == GameState.GAME) { CountDown(); } //Si estamos en juego comenzamos a contar el tiempo.
    }
    #endregion

    #region Private Method
    /// <summary>
    /// Método que refresca la UI.
    /// </summary>
    private void RefreshUI()
    {
        keyText.text = asKey ? "Key 1 / 1" : "Key 0 / 1"; //Seteamos el texto de la llave.
        secretText.text = $"Secret: {actualSecret} / {secretLevel}"; //Seteamos el texto de los secretos.
    }
    /// <summary>
    /// Método que lleva el tiempo.
    /// </summary>
    private void CountDown()
    {
        actualTime -= Time.deltaTime; //Restamos un segundo al segundo.
        if (actualTime <= 0)
        {
            ChangeGameState(GameState.GAMEOVER); //Si el tiempo es menos que 0 pasamos a GameOver.
        }
        else
        {
            string minutes = (int)actualTime / 60 < 10 ? $"0{(int)actualTime / 60}" : $"{(int)actualTime / 60}";
            string seconds = (int)actualTime % 60 < 10 ? $"0{(int)actualTime % 60}" : $"{(int)actualTime % 60}";
            
            timerText.text = $"Time Left: {minutes}:{seconds}"; //Si no seteamos el tiempo en el texto.
        }
    }
    #endregion

    #region Public Method
    /// <summary>
    /// Método para cambiar el estado del juego.
    /// </summary>
    /// <param name="newState">Nuevo estado.</param>
    public void ChangeGameState(GameState newState)
    {
        gameState = newState; //Cambiamos el estado.
    }
    /// <summary>
    /// Método para sumar el item encontrado.
    /// </summary>
    /// <param name="tag">Etiqueta del item.</param>
    public void ItemFind(string tag)
    {
        switch (tag)
        {
            case "Key": //Si es una llave.
                asKey = !asKey; //Activamos el booleano.
                exit.SetActive(true); //Acticamos la salida.
                break;
            case "Secret": //Si es un secreto.
                actualSecret++; //Sumamos uno a los secretos.
                break;
        }
        RefreshUI(); //Refrescamos la UI.
    }
    /// <summary>
    /// Método que se activa cuando ganamos.
    /// </summary>
    public void Win()
    {
        timerText.text = "You Win"; //Cambiamos la variable del tiempo.
        Time.timeScale = 0; //Paramos el tiempo.
    }
    #endregion

    #region Corroutines
    IEnumerator StartGame()
    {
        ChangeGameState(GameState.INITIAL); //Cambiamos el estado del juego.
        secretLevel = GameObject.FindGameObjectsWithTag("Secret").Length; //Buscamos el número total de secretos.
        asKey = false; //Desactivamos la llave.
        actualSecret = 0; //Inicializamos la variable del total de secretos.
        actualTime = timeLevel; //Seteamos el tiempo.
        RefreshUI(); //Resetamos la UI.
        yield return new WaitForSeconds(2f); //Esperamos 2 segundos.
        camera2.SetActive(false); //Desactivamos una cámara.
        camera1.SetActive(true); //Activamos la otra.
        yield return new WaitForSeconds(2f); //Esperamos otros 2 segundos.
        ChangeGameState(GameState.GAME); //Cambiamos el estado del juego.
    }
    #endregion
}
