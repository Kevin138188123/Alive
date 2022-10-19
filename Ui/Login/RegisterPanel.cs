using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
�û�ע����������
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
        //�Ҷ��ӵ����Input����******************************
        accInput = transform.Find("AccountInput").gameObject.GetComponent<TMP_InputField>();
        passInput = transform.Find("PasswordInput").gameObject.GetComponent<TMP_InputField>();
        pass2Input = transform.Find("PasswordInput2").gameObject.GetComponent<TMP_InputField>();
        //�Ҷ��ӵĶ��ӵ����
        accTipTxt = transform.Find("AccountInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        passTipTxt = transform.Find("PasswordInput").Find("Tip").GetComponent<TextMeshProUGUI>();
        pass2TipTxt = transform.Find("PasswordInput2").Find("Tip").GetComponent<TextMeshProUGUI>();
        //�Ҷ��ӵ����
        errorTipTxt = transform.Find("ErrorTips").GetComponent<TextMeshProUGUI>();
        registerBtn = transform.Find("RegisterBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();

        //����¼�
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
            accTipTxt.text = "�˺Ų���Ϊ�գ�";
            pass2TipTxt.text = "";
            passTipTxt.text = "";
            return;
        }
        else
        {
            accTipTxt.text = "";
            if (Login_Model.Instance.AccountExists(accInput.text))
            {
                errorTipTxt.text = "�˺��Ѵ��ڣ�";
                return;
            }
            else
            {
                errorTipTxt.text = "";
            }
        }

        if (passInput.text == "")
        {
            passTipTxt.text = "���벻��Ϊ�գ�";
            pass2TipTxt.text = "";
            return;
        }
        else
        {
            passTipTxt.text = "";
        }

        if (pass2Input.text == "")
        {
            pass2TipTxt.text = "�ٴ��������벻��Ϊ�գ�";
            return;
        }
        else
        {
            pass2TipTxt.text = "";
            if (!passInput.text.Equals(pass2Input.text))
            {
                errorTipTxt.color = Color.red;
                errorTipTxt.text = "�����������벻һ��!";
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
            errorTipTxt.text = "�˺��Ѵ��ڣ�";
            return;
        }
        else
        {
            //��ע����Ϣ�������ݿ⣨Xml��
            Login_Model.Instance.SaveLoginInfo(accInput.text, passInput.text);
            CleanTips();
            errorTipTxt.color = Color.green;
            errorTipTxt.text = "ע��ɹ��������ص�¼�˺�";
        }

    }
}

