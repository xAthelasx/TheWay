using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    #region SerializeField Variables
    [Header("Panel del tutorial")]
    [SerializeField] private GameObject panelText; //Variable que guarda el panel del tutoria.

    [Space][Header("C�maras")]
    [SerializeField] private GameObject[] cameras; //Array que guarda todas las c�maras por las que nos vamos a mover.

    [Space][Header("GameObjects UI")]
    [SerializeField] private TMP_Text tutorialText; //Texto a rellenar con la informaci�n del tutorial.
    [SerializeField] private GameObject timeImage; //Variable que guarda la animaci�n del timer.
    #endregion

    #region Monobehaviour Method
    private void Start()
    {
        panelText.SetActive(false);
        timeImage.SetActive(false);
        Invoke("StartTutorial", 4.5f); //Esperamos un tiempo antes de empezar el tutorial.
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine(Initial());
            panelText.SetActive(false);
            ChangeCamera(0);
            LevelManager.instance.ChangeGameState(GameState.GAME);
            this.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Private Method
    private void StartTutorial() { StartCoroutine(Initial()); } //Lanzamos la corrutina.

    /// <summary>
    /// M�todo que se encarga de cambiar la c�mara activa.
    /// </summary>
    /// <param name="camera">Camara a encender</param>
    private void ChangeCamera(int camera)
    {
        for(int i = 0; i < cameras.Length; i++) //Recorremos el array de c�maras.
        {
            cameras[i].SetActive(i == camera ? true : false); //Si la posici�n del array corresponde con la c�mara la activamos.
        }
    }

    #endregion

    #region Corroutines
    /// <summary>
    /// Corrutina del tutorial.
    /// </summary>
    IEnumerator Initial()
    {
        LevelManager.instance.ChangeGameState(GameState.PAUSE); //Ponemos el juego en pause para que no corra el tiempo.
        panelText.SetActive(true); //Activamos el panel del tutorial.
        yield return StartCoroutine(Message("Bienvenido a: The Way")); //Lanzamos mensaje.
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); //Esperamos a que se pulse espacio.
        ChangeCamera(1); //Cambiamos la c�mara.
        yield return StartCoroutine(Message("En The Way tienes que encontrar la llave que te permitir� salir del laberinto.")); // Lanzamos mensaje.
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        timeImage.SetActive(true); //Activamos la animaci�n 
        yield return StartCoroutine(Message("Para ello tienes un tiempo determinado que correr� en contra tuya."));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        timeImage.SetActive(false);
        ChangeCamera(3);
        yield return StartCoroutine(Message("Adicionalmente cada mapa tiene secretos que ayudar�n a mejorar la puntuaci�n en el nivel."));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        yield return StartCoroutine(Message("Utiliza AWSD o las flechas de control para mover al personaje."));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        ChangeCamera(0);
        yield return StartCoroutine(Message("��SUERTE!!"));
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        panelText.SetActive(false); //Desactivamos el panel.
        LevelManager.instance.ChangeGameState(GameState.GAME); //Ponemos el juego de nuevo activo.
        this.gameObject.SetActive(false);

    }
    /// <summary>
    /// Corrutina para escribir letra a letra.
    /// </summary>
    /// <param name="msg">Mensaje a escribir.</param>
    IEnumerator Message(string msg)
    {
        tutorialText.text = ""; //Ponemos el panel limpio.
        foreach (char tmp in msg) //Recorremos los caracteres del mensaje.
        {
            tutorialText.text += tmp; //Los sumamos uno a uno al mensaje del panel.
            yield return new WaitForSeconds(0.05f); //Esperamos 0.5 segundos.
        }
    }
    #endregion
}
