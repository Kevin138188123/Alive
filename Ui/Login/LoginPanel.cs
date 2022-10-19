using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
��¼��壬��¼������
*/
public class LoginPanel : MonoBehaviour
{
    Button loginBtn;
    Button registerBtn;
    TMP_InputField accInput;
    TMP_InputField passInput;
    TextMeshProUGUI accTipTxt;
    TextMeshProUGUI passTipTxt;
    TextMeshProUGUI errorTipTxt;
    public Transform registerPanel;
    public Transform choosePanel;
    public Transform createPanel;

    private void Awake()
    {
        //�Ҷ��ӵ����Input����******************************
        accInput = transform.Find("AccountInput").gameObject.GetComponent<TMP_InputField>();
        passInput = transform.Find("PasswordInput").gameObject.GetComponent<TMP_InputField>();
        //�Ҷ��ӵĶ��ӵ����
        accTipTxt = transform.Find("AccountInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        passTipTxt = transform.Find("PasswordInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        //�Ҷ��ӵ����
        errorTipTxt = transform.Find("ErrorTips").GetComponent<TextMeshProUGUI>();
        loginBtn = transform.Find("LoginBtn").GetComponent<Button>();
        registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();

        Init();
    }

    private void OnEnable()
    {
        CleanTips();
    }

    public void Init()
    {
        loginBtn.onClick.AddListener(OnClickLoginBtn);
        registerBtn.onClick.AddListener(OnClickRegisterBtn);
        CleanTips();
    }

    public void CleanTips()
    {
        accInput.text = "";
        passInput.text = "";
        accTipTxt.text = "";
        passTipTxt.text = "";
        errorTipTxt.text = "";
    }
    private void OnClickRegisterBtn()
    {
        CleanTips();
        gameObject.SetActive(false);
        registerPanel.gameObject.SetActive(true);

    }

    private void OnClickLoginBtn()
    {
        if (accInput.text == "")
        {
            accTipTxt.text = "�˺Ų���Ϊ�գ�";
            return;
        }
        else
        {
            accTipTxt.text = "";
        }

        if (passInput.text == "")
        {
            passTipTxt.text = "���벻��Ϊ�գ�";
            return;
        }
        else
        {
            passTipTxt.text = "";
        }
        
        if (!Login_Model.Instance.AccountExists(accInput.text))
        {
            errorTipTxt.text = "�˺Ų����ڣ�";
            return;
        }
        else
        {
            errorTipTxt.text = "";
        }

        if (!Login_Model.Instance.Matching(accInput.text, passInput.text))
        {
            errorTipTxt.text = "�˺Ż��������";
        }
        else
        {
            //1.�����˺���Ϣ�ڱ���Ӳ��
            //PlayerPrefs.SetString("Account",accInput.text);
            //2.����¼��Ϣ�����ڳ�����ľ�̬�����У��˻���������ѡ��
            //AppConst.accountName=accInput.text;
            //3.�����˻����ƶ�ȡ�˻�Id��������Model��ı�����
            Login_Model.Instance.GetAccountIdByName(accInput.text);
            //����LoginModel�㱣����˻�Id��ȡ�˻��µĽ�ɫ�б�����CharacterMoldel������
            var count=CharList_Model.Instance.LoadCharacterByAccountId(Login_Model.Instance.accountId);
            if (count > 0)
            {
                choosePanel.gameObject.SetActive(true);
                createPanel.gameObject.SetActive(false);
            }
            else
            {
                choosePanel.gameObject.SetActive(false);
                createPanel.gameObject.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}

