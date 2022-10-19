using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
��Ϸ������������
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
        soundSlider = GetComponentInChildren<Slider>();//Ψһ���ʱ
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();//���ͬ����Ҫ����
        progressTxt = transform.Find("ProgressTxt").GetComponent<TextMeshProUGUI>();

        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        soundSlider.onValueChanged.AddListener(OnValueChanged);
        Init();
    }

    private void Init()
    {
        if (PlayerPrefs.HasKey("BackgroundSoundVolume"))
        {
            //���������VideoPlayer���ʱ����ʼ���ı�����������ÿ�������޷���ȡ�ı�����
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

