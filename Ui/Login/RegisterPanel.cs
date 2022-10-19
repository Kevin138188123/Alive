using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
用户注册面板控制器
*/
public class RegisterPanel : MonoBehaviour
{
    Button backBtn;
    Button registerBtn;
    TMP_InputField accInput;
    TMP_InputField passInput;
    TMP_InputField pass2Input;
    TextMeshProUGUI accTipTxt;
    TextMeshProUGUI passTipTxt;
    TextMeshProUGUI pass2TipTxt;
    TextMeshProUGUI errorTipTxt;
    public Transform loginPanel;

    private void Awake()
    {
        //找儿子的组件Input特殊******************************
        accInput = transform.Find("AccountInput").gameObject.GetComponent<TMP_InputField>();
        passInput = transform.Find("PasswordInput").gameObject.GetComponent<TMP_InputField>();
        pass2Input = transform.Find("PasswordInput2").gameObject.GetComponent<TMP_InputField>();
        //找儿子的儿子的组件
        accTipTxt = transform.Find("AccountInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        passTipTxt = transform.Find("PasswordInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        pass2TipTxt = transform.Find("PasswordInput2").Find("Tip").GetComponent<TextMeshProUGUI>();
        //找儿子的组件
        errorTipTxt = transform.Find("ErrorTips").GetComponent<TextMeshProUGUI>();
        registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        //添加事件
        backBtn.onClick.AddListener(OnClickBackBtn);
        registerBtn.onClick.AddListener(OnClickRegisterBtn);

        CleanTips();
    }

    public void CleanTips()
    {
        accInput.text = "";
        passInput.text = "";
        pass2Input.text = "";
        accTipTxt.text = "";
        passTipTxt.text = "";
        pass2TipTxt.text = "";
        errorTipTxt.text = "";
    }

    private void OnClickBackBtn()
    {
        CleanTips();
        gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
    }

    private void OnClickRegisterBtn()
    {
        if (accInput.text == "")
        {
            accTipTxt.text = "账号不能为空！";
            pass2TipTxt.text = "";
            passTipTxt.text = "";
            return;
        }
        else
        {
            accTipTxt.text = "";
            if (Login_Model.Instance.AccountExists(accInput.text))
            {
                errorTipTxt.text = "账号已存在！";
                return;
            }
            else
            {
                errorTipTxt.text = "";
            }
        }

        if (passInput.text == "")
        {
            passTipTxt.text = "密码不能为空！";
            pass2TipTxt.text = "";
            return;
        }
        else
        {
            passTipTxt.text = "";
        }

        if (pass2Input.text == "")
        {
            pass2TipTxt.text = "再次输入密码不能为空！";
            return;
        }
        else
        {
            pass2TipTxt.text = "";
            if (!passInput.text.Equals(pass2Input.text))
            {
                errorTipTxt.color = Color.red;
                errorTipTxt.text = "两次密码输入不一致!";
                return;
            }
            else
            {
                errorTipTxt.text = "";
            }

        }
        if (Login_Model.Instance.AccountExists(accInput.text))
        {
            errorTipTxt.color = Color.red;
            errorTipTxt.text = "账号已存在！";
            return;
        }
        else
        {
            //将注册信息存入数据库（Xml）
            Login_Model.Instance.SaveLoginInfo(accInput.text, passInput.text);
            CleanTips();
            errorTipTxt.color = Color.green;
            errorTipTxt.text = "注册成功，按返回登录账号";
        }

    }
}

