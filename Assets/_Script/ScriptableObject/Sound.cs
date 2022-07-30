using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType { FX, MUSIC }
[CreateAssetMenu(fileName = "Sound", menuName = "Sound/sound")]
public class Sound : ScriptableObject
{
    #region Variables
    public AudioClip clip;
    public SoundType soundtype;
    public bool loop;
    #endregion

}
