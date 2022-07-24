using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager Instance;
	[SerializeField] private AudioSource _musicSource, effectsSource;
	void Awake()
	{
		if(Instance  == null) 
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		} else 
		{
			Destroy(gameObject);
		}
	}
}
