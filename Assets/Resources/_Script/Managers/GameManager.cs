using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Static Variables
    public static GameManager instance; //Variable del singleton.
    #endregion

    #region Private Variables
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource fxSource;

    #endregion

    #region Public Variables
    public AudioSource MusicSource 
    { 
        get { return musicSource; } 
        set { musicSource = value; }
    }
    public AudioSource FxSource 
    { 
        get { return fxSource; } 
        set { fxSource = value; }
    }
    #endregion

    #region Monobehaviour Method
    private void Awake()
    {
        if(instance == null) //Si la variable está vacía.
        {
            instance = this; //Es igual a todo lo de aquí.
            DontDestroyOnLoad(this.gameObject); //No destruimos el objeto entre escenas.
        }
        else //Si ya tiene algo.
        {
            Destroy(this.gameObject); //La destruimos.
        }
    }
    private void Start()
    {
        LoadConfiguration();   
    }
    #endregion
    #region Public Method
    /// <summary>
    /// Método que se encarga de el audio en el juego.
    /// </summary>
    /// <param name="sound">Scriptable con el archivo a reproducir.</param>
    public void PlayAudio(Sound sound)
    {
        if(sound.soundtype == SoundType.MUSIC) { PlayMusic(sound); }
        else { PlayFx(sound); }
    }
    /// <summary>
    /// Método que se encarga de cambiar entre escenas.
    /// </summary>
    /// <param name="index">Index de la escena a cambiar.</param>
    public void SceneChange(int index)
    {
        musicSource.Stop();
        SceneManager.LoadScene(index); //Llamamos al cambio de escena.
    }
    /// <summary>
    /// Método para quitar el juego.
    /// </summary>
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit(); 
    }

    public void Save(int vSync)
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
        PlayerPrefs.SetFloat("FxVolume", fxSource.volume);
        PlayerPrefs.SetInt("Vsync", vSync);
    }
    #endregion
    #region Private variables
    /// <summary>
    /// Método usado en caso de que el audio sea música.
    /// </summary>
    private void PlayMusic(Sound sound)
    {
        musicSource.clip = sound.clip;
        musicSource.loop = sound.loop;
        musicSource.Play();
    }
    /// <summary>
    /// Método usado en caso de que el audio sea fx.
    /// </summary>
    private void PlayFx(Sound sound)
    {
        if (FxSource.isPlaying) { return; }
        fxSource.clip = sound.clip;
        fxSource.loop = sound.loop;
        fxSource.Play();
    }

    private void LoadConfiguration()
    {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        fxSource.volume = PlayerPrefs.GetFloat("FxVolume");
        if(PlayerPrefs.GetInt("Vsync") == 1) { Application.targetFrameRate = 60; }
    }
    #endregion
}
