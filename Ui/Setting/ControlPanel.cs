using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
界面控制面板
*/
public class ControlPanel : MonoBehaviour
{
    public Transform CharacterPanel;
    public Transform BagPanel;

    Button charBtn;
    Button bagBtn;
    Button skillBtn;

    private void Awake()
    {
        charBtn = transform.Find("CharBtn_Mask").Find("CharBtn").GetComponent<Button>();
        bagBtn = transform.Find("BagBtn_Mask").Find("BagBtn").GetComponent<Button>();
        skillBtn = transform.Find("SkillBtn_Mask").Find("SkillBtn").GetComponent<Button>();
        charBtn.onClick.AddListener(OnClickCharBtn);
        bagBtn.onClick.AddListener(OnClickBagBtn);
    }

    private void OnClickCharBtn()
    {
        if (CharacterPanel.gameObject.activeInHierarchy)
        {
            CharacterPanel.gameObject.SetActive(false);
        }
        else
        {
            CharacterPanel.gameObject.SetActive(true);
        }
    }
    private void OnClickBagBtn()
    {
        if (BagPanel.gameObject.activeInHierarchy)
        {
            BagPanel.gameObject.SetActive(false);
        }
        else
        {
            BagPanel.gameObject.SetActive(true);
        }
        
    }
    private void OnClickSkillBtn()
    {
        throw new NotImplementedException();
    }
}

