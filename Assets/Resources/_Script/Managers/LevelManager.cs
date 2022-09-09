using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum GameState { INITIAL, GAME, PAUSE, GAMEOVER} //Enumerado con los momentos de juego.
public class LevelManager : MonoBehaviour
{
    #region SerializeField Variables
    [Header("Variables de GameObject")]
    [SerializeField] GameObject camera1; //Variables con las cámaras.
    [SerializeField] GameObject camera2;
    [SerializeField] GameObject exit; //Variable para la salida.
    [SerializeField] GameObject panelWin; //Variable del panel de victoria/derrota.
    [Space][Header("Tiempo total del nivel")]
    [SerializeField] float timeLevel; //Variable que nos indicará el tiempo del nivel.
    [Space][Header("Textos")]
    [SerializeField] TMP_Text keyText; //Texto de conteo de llave.
    [SerializeField] TMP_Text secretText; //Texto de conteo de Secretos.
    [SerializeField] TMP_Text timerText; //Texto de tiempo.
    [Space]
    [Header("Victoria/Derrota")]
    [SerializeField] TMP_Text victoryText; //Texto que indica si hemos ganado o perdido.
    [SerializeField] Button menuButton; //Botón que nos devuelve al menú.
    [SerializeField] float finalTime; //Variable que indica que tiempo tenemos que hacer en la pantalla.
    [SerializeField] int level; //Variable que contendrá el nivel al que corresponde el script.
    [Space][Header("Sonidos")]
    [SerializeField] Sound keySound; //Variable de sonido de la llave.
    [SerializeField] Sound secretSound; //Variable de sonido del secreto.
    [SerializeField] Sound winSound; //Variable de sonido de victoria.
    [SerializeField] Sound loseSound; //Variable de sonido de derrota.
    [SerializeField] Sound sceneSound; //Variable con la música de la escena.
    #endregion

    #region Private Variables
    GameState gameState; //Variable de estado de juego.
    bool asKey; //Booleano que nos indica si tenemos la llave.
    float secretLevel; //Booleano que nos cuenta el número de secretos que hay en el nivel.
    float actualSecret; //Número de secrertos encontrados.
    float actualTime; //Tiempo actual.
    AudioSource levelSound; //Audiosource del nivel.
    #endregion

    #region Public Variables
    public GameState GameState { get { return gameState; } } //Variable que nos devuelve el estado del juego.
    public static LevelManager instance;
    #endregion

    #region MonoBehaviour Method
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this.gameObject); }
    }
    private void Start()
    {
        levelSound = GetComponent<AudioSource>();
        ChangeTimeScale(1);
        menuButton.onClick.AddListener(() => GameManager.instance.SceneChange(0));
        StartCoroutine(StartGame()); //Iniciamos la corrutina de empezar el juego.
    }

    private void Update()
    {
        if(gameState == GameState.GAME) { CountDown(); } //Si estamos en juego comenzamos a contar el tiempo.
        if (Input.GetKeyDown(KeyCode.Escape)) { PauseGame(); }
    }
    #endregion

    #region Private Method
    private void ChangeTimeScale(int scale)
    {
        Time.timeScale = scale;
    }
    private void PauseGame()
    {
        if (gameState == GameState.GAME)
        {
            ChangeTimeScale(0);
            ChangeGameState(GameState.PAUSE);
            timerText.text = "Pause";
        }
        else
        {
            ChangeTimeScale(1);
            ChangeGameState(GameState.GAME);
        }
    }
    /// <summary>
    /// Método que refresca la UI.
    /// </summary>
    private void RefreshUI()
    {
        keyText.text = asKey ? "Key: 1 / 1" : "Key: 0 / 1"; //Seteamos el texto de la llave.
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
            GameOver();
        }
        else
        {
            string minutes = (int)actualTime / 60 < 10 ? $"0{(int)actualTime / 60}" : $"{(int)actualTime / 60}";
            string seconds = (int)actualTime % 60 < 10 ? $"0{(int)actualTime % 60}" : $"{(int)actualTime % 60}";
            
            timerText.text = $"Time Left: {minutes}:{seconds}"; //Si no seteamos el tiempo en el texto.
        }
    }

    private void SaveNewScore(int stars)
    {
        int tmp = PlayerPrefs.GetInt("level" + level);
        if (stars > tmp) { PlayerPrefs.SetInt("level" + level, stars); }
    }

    private void PlaySound(Sound sound)
    {
        levelSound.clip = sound.clip;
        levelSound.loop = sound.loop;
        levelSound.Play();
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
                PlaySound(keySound);
                asKey = !asKey; //Activamos el booleano.
                exit.SetActive(true); //Acticamos la salida.
                break;
            case "Secret": //Si es un secreto.
                PlaySound(secretSound);
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
        ChangeGameState(GameState.GAMEOVER);
        PlaySound(winSound);
        timerText.text = "You Win"; //Cambiamos la variable del tiempo.
        GameManager.instance.MusicSource.Stop();
        ChangeTimeScale(0); //Paramos el tiempo.
        StartCoroutine(Final(0));
    }

    public void GameOver()
    {
        ChangeGameState(GameState.GAMEOVER);
        PlaySound(loseSound);
        GameManager.instance.MusicSource.Stop();
        timerText.text = "You Lose";
        ChangeTimeScale(0);
        StartCoroutine(Final(1));
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
        if (level > 0) { ChangeGameState(GameState.GAME); } //Cambiamos el estado del juego.
        GameManager.instance.PlayAudio(sceneSound);
    }

    IEnumerator Final(int pass)
    {
        int complete = 1;
        int totalStars = pass == 0 ? 1 : 0;
        panelWin.SetActive(true);
        victoryText.text = pass == 0 ? "You Win" : "You Lose";
        GameObject.Find("LevelCompleteText").transform.GetChild(pass).gameObject.SetActive(true);
        complete = ((secretLevel == actualSecret) && pass == 0) ? 0 : 1;
        totalStars += complete == 0 ? 1 : 0;
        GameObject.Find("SecretFoundText").transform.GetChild(complete).gameObject.SetActive(true);
        complete = ((timeLevel - actualTime > finalTime) && pass == 0) ? 0 : 1;
        totalStars += complete == 0 ? 1 : 0;
        GameObject.Find("TimeLeftText").transform.GetChild(complete).gameObject.SetActive(true);
        SaveNewScore(totalStars);
        yield return null;
    }
    #endregion
}
