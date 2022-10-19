using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
��¼ģ�Ͳ㣬��ȡ���洢��¼����,ע���˺�
*/
public class Login_Model
{
    //�˻�Id���⽻��
    public string accountId;
    //����xml·��
    string accountInfoXml;
    Login_Model()
    {
        accountInfoXml = AppConst.accountInfoXml;
    }
    public static readonly Login_Model Instance = new Login_Model();

    //����ע�����˻�Id
    public void GetAccountIdByName(string _accountName)
    {
        if (!File.Exists(AppConst.accountInfoXml))
        {
            Debug.LogError("û���˻���Ϣ�ļ�(Xml)");
            return;
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(accountInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            foreach (XmlElement account in root.ChildNodes)
            {
                if (account.GetAttribute("name") == _accountName)
                {
                    accountId = account.GetAttribute("id");
                }
            }
        }
    }

    //ע��
    public void SaveLoginInfo(string _name, string _password)
    {
        XmlDocument doc = new XmlDocument();
        var account = doc.CreateElement("Account");//����һ����ǩ
        //�ֶ�����id
        var id = Random.Range(10000, 99999).ToString();
        account.SetAttribute("id", id);
        account.SetAttribute("name", _name);//Ϊһ����ǩ�������
        account.SetAttribute("password", _password);
        if (File.Exists(accountInfoXml))
        {
            doc.Load(accountInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;//��ȡ��Ŀ¼
            root.AppendChild(account);
        }
        else
        {
            var root = doc.CreateElement("Root");//�½���Ŀ¼
            doc.AppendChild(root);
            root.AppendChild(account);
        }
        doc.Save(accountInfoXml);
    }

    //�ж��˻��Ƿ����
    public bool AccountExists(string _account)
    {
        if (!File.Exists(accountInfoXml))
        {
            Debug.LogError("accountInfo�ļ�δ�ҵ�");
            return false;
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(accountInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            foreach (XmlElement account in root.ChildNodes)
            {
                if (account.GetAttribute("name") == _account)
                {
                    return true;
                }
            }
            return false;
        }
    }

    //��֤�˻�����
    public bool Matching(string _account, string _password)
    {
        if (!File.Exists(accountInfoXml))
        {
            Debug.LogError("accountInfo�ļ�δ�ҵ�");
            return false;
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(accountInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            foreach (XmlElement account in root.ChildNodes)
            {
                if (account.GetAttribute("name") == _account)
                {
                    if (account.GetAttribute("password") == _password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }


}

