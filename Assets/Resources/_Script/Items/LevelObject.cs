using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelObject : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] TMP_Text levelNumber;
    [SerializeField] GameObject[] imageStars;
    [SerializeField] Sprite starUp, starDown;
    [SerializeField] Button playButton;
    #endregion
    #region Monobehaviour Method

    #endregion
    #region Public Method
    public void FillObject(int level, int stars, bool open)
    { 
        levelNumber.text = level == 0 ? $"Tutorial" : $"Level: {level}";
        for (int i=0; i< imageStars.Length; i++)
        {
            if (stars > i) { imageStars[i].GetComponent<Image>().sprite = starUp; }
            else { imageStars[i].GetComponent<Image>().sprite = starDown; }
        }
        if (open) { playButton.interactable = true; }
        else { playButton.interactable = false; }
        playButton.onClick.AddListener(() => GameManager.instance.SceneChange(level + 1));
    }
    #endregion
}
