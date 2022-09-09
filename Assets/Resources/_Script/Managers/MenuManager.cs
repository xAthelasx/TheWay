using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region SerializeField variables
    [SerializeField] GameObject[] panels;
    [SerializeField] Sound musicMenu;

    [SerializeField] GameObject panelLevel;
    [SerializeField] int numberOfLevels = 5;

    [SerializeField] Button quitButton;
    #endregion

    #region Monobehaviour Method
    private void Start()
    {
        quitButton.onClick.AddListener(() => GameManager.instance.QuitGame());
        GameManager.instance.PlayAudio(musicMenu);
        FillLevels();
    }
    #endregion

    #region Public Method
    public void ChangeActivePanel(int index)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        if (index == -1) { return; }
        else { panels[index].SetActive(true); }
    }
    #endregion

    #region Private Method
    void FillLevels()
    {
        int tmpStars = 1;
        bool levelComplete = false;
        for (int i = 0; i < numberOfLevels; i++)
        {
            if (tmpStars > 0) { levelComplete = true; }
            else { levelComplete = false; }
            tmpStars = PlayerPrefs.GetInt("level" + i);
            GameObject tmp = Instantiate(Resources.Load("Prefab/Level") as GameObject);
            tmp.name = $"Level{i}";
            tmp.transform.SetParent(panelLevel.transform);
            tmp.transform.position = Vector3.zero;
            tmp.GetComponent<LevelObject>().FillObject(i, tmpStars, levelComplete);
        }
    }
    #endregion
}
