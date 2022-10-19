using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
头像信息面板
*/
public class HeadPanel : MonoBehaviour
{
    TextMeshProUGUI nameTxt;
    TextMeshProUGUI levelTxt;
    Transform hpBar;
    Transform mpBar;
    Transform expBar;


    private void Awake()
    {
        nameTxt = transform.Find("NameTxt").GetComponent<TextMeshProUGUI>();
        levelTxt = transform.Find("LevelTxt").GetComponent<TextMeshProUGUI>();
        hpBar = transform.Find("HpBar").GetComponent<Transform>(); ;
        mpBar = transform.Find("MpBar").GetComponent<Transform>(); ;
        expBar = transform.Find("ExpBar").GetComponent<Transform>(); ;
        UpdateHeadPanel();
        MessageEventSystem.Instance.AddListener(EventMessage.hpUpdateMes,UpdateHeadPanel);
    }

    private void UpdateHeadPanel()
    {
        var charPro = Character_Model.Instancce.character;
        if (charPro == null)
        {
            Debug.LogError("未加载到角色信息");
            return;
        }
        nameTxt.text = charPro.Name;
        levelTxt.text = "Lv." + charPro.Level.ToString();
        hpBar.Find("Fill").GetComponent<Image>().fillAmount = charPro.Hp * 1f / charPro.MaxHp;
        hpBar.Find("HpTxt").GetComponent<TextMeshProUGUI>().text = (charPro.Hp * 100.0f / charPro.MaxHp).ToString("00") + "%";
        mpBar.Find("Fill").GetComponent<Image>().fillAmount = charPro.Mp * 1f / charPro.MaxMp;
        mpBar.Find("MpTxt").GetComponent<TextMeshProUGUI>().text = (charPro.Mp * 100.0f / charPro.MaxMp).ToString("00") + "%";
        expBar.Find("Fill").GetComponent<Image>().fillAmount = charPro.Exp * 1f / charPro.MaxExp;
        expBar.Find("ExpTxt").GetComponent<TextMeshProUGUI>().text = (charPro.Exp * 100.0f / charPro.MaxExp).ToString("00") + "%";
    }


}

