using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
登录模型层，读取、存储登录数据,注册账号
*/
public class Login_Model
{
    //账户Id对外交互
    public string accountId;
    //保存xml路径
    string accountInfoXml;
    Login_Model()
    {
        accountInfoXml = AppConst.accountInfoXml;
    }
    public static readonly Login_Model Instance = new Login_Model();

    //保留注册后的账户Id
    public void GetAccountIdByName(string _accountName)
    {
        if (!File.Exists(AppConst.accountInfoXml))
        {
            Debug.LogError("没有账户信息文件(Xml)");
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

    //注册
    public void SaveLoginInfo(string _name, string _password)
    {
        XmlDocument doc = new XmlDocument();
        var account = doc.CreateElement("Account");//创建一个标签
        //手动生成id
        var id = Random.Range(10000, 99999).ToString();
        account.SetAttribute("id", id);
        account.SetAttribute("name", _name);//为一个标签添加属性
        account.SetAttribute("password", _password);
        if (File.Exists(accountInfoXml))
        {
            doc.Load(accountInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;//读取根目录
            root.AppendChild(account);
        }
        else
        {
            var root = doc.CreateElement("Root");//新建根目录
            doc.AppendChild(root);
            root.AppendChild(account);
        }
        doc.Save(accountInfoXml);
    }

    //判断账户是否存在
    public bool AccountExists(string _account)
    {
        if (!File.Exists(accountInfoXml))
        {
            Debug.LogError("accountInfo文件未找到");
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

    //验证账户密码
    public bool Matching(string _account, string _password)
    {
        if (!File.Exists(accountInfoXml))
        {
            Debug.LogError("accountInfo文件未找到");
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

