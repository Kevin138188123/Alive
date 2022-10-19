using System.Collections.Generic;
using UnityEngine;
/*
��ȡ���������ĵ�PlayerPrefab
*/
public class LoadPlayerPrefab : MonoBehaviour
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.HasKey("BackgroundSoundVolume"))
        {
            audioSource.volume = PlayerPrefs.GetFloat("BackgroundSoundVolume");
        }
    }


}

