using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
游戏设置面板控制器
*/
public class SettingPanel : MonoBehaviour
{
    Slider soundSlider;
    Button confirmBtn;
    TextMeshProUGUI progressTxt;
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = Camera.main.gameObject.GetComponent<AudioSource>();
        soundSlider = GetComponentInChildren<Slider>();//唯一组件时
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();//多个同类需要区别
        progressTxt = transform.Find("ProgressTxt").GetComponent<TextMeshProUGUI>();

        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        soundSlider.onValueChanged.AddListener(OnValueChanged);
        Init();
    }

    private void Init()
    {
        if (PlayerPrefs.HasKey("BackgroundSoundVolume"))
        {
            //主摄像机挂VideoPlayer组件时：初始化改变音量，否则每次运行无法读取改变音量
            //video.SetDirectAudioVolume(0, slider.value);
            //********************************
            soundSlider.value = PlayerPrefs.GetFloat("BackgroundSoundVolume");
            progressTxt.text = (soundSlider.value * 100).ToString("F0") + "%";
        }
    }

    private void OnClickConfirmBtn()
    {
        gameObject.SetActive(false);
    }

    public void OnValueChanged(float _value)
    {
        progressTxt.text = (_value * 100).ToString("F0") + "%";
        if (audioSource != null)
        {
            audioSource.volume = _value;
            PlayerPrefs.SetFloat("BackgroundSoundVolume", _value);
        }
    }
}

