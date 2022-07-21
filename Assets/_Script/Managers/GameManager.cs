using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Static Variables
    public static GameManager instance;
    #endregion

    #region Monobehaviour Method
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    #region Public Method
    public void SceneChange(int index)
    {
        SceneManager.LoadScene(index);
    }
    #endregion
}
