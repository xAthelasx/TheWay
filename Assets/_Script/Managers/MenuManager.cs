using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    #region SerializeField variables
    [SerializeField] GameObject[] panels;
    #endregion

    #region Monobehaviour Method

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
}
