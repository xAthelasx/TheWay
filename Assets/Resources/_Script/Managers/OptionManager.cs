using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    #region Serializable Variables
    [SerializeField] Slider musicSlider; 
    [SerializeField] Slider fxSlider;
    [SerializeField] Toggle vSinc;
    #endregion

    #region Monobehaviour method
    private void Awake()
    {
        musicSlider.value = GameManager.instance.MusicSource.volume;
        fxSlider.value = GameManager.instance.FxSource.volume;
        if (PlayerPrefs.GetInt("Vsync") == 1) { vSinc.isOn = true; }
        else { vSinc.isOn = false; }
    }
    #endregion

    #region Public Method
    public void ChangeValues(string value)
    {
        if (value == "music") { GameManager.instance.MusicSource.volume = musicSlider.value; }
        if (value == "fx") { GameManager.instance.FxSource.volume = fxSlider.value; }
        if (vSinc) { GameManager.instance.Save(1); }
        else { GameManager.instance.Save(0); }
    }
    #endregion
}
