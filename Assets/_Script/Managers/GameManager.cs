using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Static Variables
    public static GameManager instance; //Variable del singleton.
    #endregion

    #region Monobehaviour Method
    private void Awake()
    {
        if(instance == null) //Si la variable est� vac�a.
        {
            instance = this; //Es igual a todo lo de aqu�.
            DontDestroyOnLoad(this.gameObject); //No destruimos el objeto entre escenas.
        }
        else //Si ya tiene algo.
        {
            Destroy(this.gameObject); //La destruimos.
        }
    }
    #endregion
    #region Public Method
    /// <summary>
    /// M�todo que se encarga de cambiar entre escenas.
    /// </summary>
    /// <param name="index">Index de la escena a cambiar.</param>
    public void SceneChange(int index)
    {
        SceneManager.LoadScene(index); //Llamamos al cambio de escena.
    }
    /// <summary>
    /// M�todo para quitar el juego.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit(); 
    }
    #endregion
}
