using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
登录面板，登录控制器
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
        //找儿子的组件Input特殊******************************
        accInput = transform.Find("AccountInput").gameObject.GetComponent<TMP_InputField>();
        passInput = transform.Find("PasswordInput").gameObject.GetComponent<TMP_InputField>();
        //找儿子的儿子的组件
        accTipTxt = transform.Find("AccountInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        passTipTxt = transform.Find("PasswordInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        //找儿子的组件
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
            accTipTxt.text = "账号不能为空！";
            return;
        }
        else
        {
            accTipTxt.text = "";
        }

        if (passInput.text == "")
        {
            passTipTxt.text = "密码不能为空！";
            return;
        }
        else
        {
            passTipTxt.text = "";
        }
        
        if (!Login_Model.Instance.AccountExists(accInput.text))
        {
            errorTipTxt.text = "账号不存在！";
            return;
        }
        else
        {
            errorTipTxt.text = "";
        }

        if (!Login_Model.Instance.Matching(accInput.text, passInput.text))
        {
            errorTipTxt.text = "账号或密码错误！";
        }
        else
        {
            //1.保存账号信息在本地硬盘
            //PlayerPrefs.SetString("Account",accInput.text);
            //2.将登录信息保存在常量类的静态变量中（账户、密码任选）
            //AppConst.accountName=accInput.text;
            //3.根据账户名称读取账户Id并保存在Model层的变量中
            Login_Model.Instance.GetAccountIdByName(accInput.text);
            //根据LoginModel层保存的账户Id读取账户下的角色列表并存在CharacterMoldel数据中
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

