using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnGameManager : MonoBehaviour
{
	public void ExitGame() 
	{
		SceneManager.LoadScene(0);
		SceneManager.UnloadSceneAsync("Tutorial");
	}
}
