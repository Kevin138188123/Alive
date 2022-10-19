using System.Collections.Generic;
using UnityEngine;
/*
全局静态常量类
*/
public class AppConst
{
    public static string enemyInfoXmlUrl = Application.dataPath + "/Alive/Xml/EnemyProInfo.xml";
    public static string heroInfoXmlUrl = Application.dataPath + "/Alive/Xml/HeroProInfo.xml";
    public static string accountInfoXml= Application.dataPath + "/Alive/Xml/AccountInfo.xml";
    public static string characterInfoXml = Application.dataPath + "/Alive/Xml/CharacterInfo.xml";
    public static string itemsInfoXml = Application.dataPath + "/Alive/Xml/ItemProInfo.xml";
    public static string bagInfoXml = Application.dataPath + "/Alive/Xml/BagInfo.xml";
    public static string equipInfoXml = Application.dataPath + "/Alive/Xml/EquipInfo.xml";

    //玩家登录账户信息
    //public static string characterId = ""; 
    public static int charId;
    public static bool isLogin=false;
}

public struct EventMessage
{
    public const string hpUpdateMes = "HpUpdateMes";
    public const string mpUpdateMes = "MpUpdateMes";
    public const string attributeUpdateMes = "AttributeUpdateMes";
}

public struct SubmitType
{
    public const string exitGame = "ExitGame";
    public const string removeChar = "RemoveChar";
    public const string dropItem = "DropItem";
    public const string death = "Death";
}

