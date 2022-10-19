using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
玩家工厂,生产三种不同类型的默认属性角色（创建角色时）
*/
public class Character_Factory
{
    Dictionary<int, Character_Product> heroProLib = null;
    Character_Factory()
    {
        if (heroProLib == null)
        {
            heroProLib = new Dictionary<int, Character_Product>();
        }
    }
    public static readonly Character_Factory Instance=new Character_Factory();

    public void LoadXml(string _url)
    {
        if (File.Exists(_url))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_url);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            foreach (XmlElement hero in root.ChildNodes)
            {
                Character_Product heroPro = null;
                Character_Product.CharType type = (Character_Product.CharType)int.Parse(hero.GetAttribute("type"));
                switch (type)
                {
                    case Character_Product.CharType.CT_Defender:
                        //拓展
                        Defender_Product defenderPro = new Defender_Product();

                        heroPro = defenderPro;
                        break;
                    case Character_Product.CharType.CT_Melee:
                        //拓展
                        Melee_Product meleePro = new Melee_Product();

                        heroPro = meleePro;
                        break;
                    case Character_Product.CharType.CT_Archer:
                        //拓展
                        Archer_Product archerPro = new Archer_Product();

                        heroPro = archerPro;
                        break;
                    default:
                        break;
                }
                heroPro.Id = int.Parse(hero.GetAttribute("id"));
                heroPro.AccountId = hero.GetAttribute("accountId");//空
                heroPro.Type = type;
                heroPro.Level = int.Parse(hero.GetAttribute("level"));
                heroPro.TypeName = hero.GetAttribute("typeName");
                heroPro.Name = hero.GetAttribute("name");//空
                heroPro.MaxHp = int.Parse(hero.GetAttribute("maxHp"));
                heroPro.MaxMp = int.Parse(hero.GetAttribute("maxMp"));
                heroPro.Atk = int.Parse(hero.GetAttribute("atk"));
                heroPro.Def = int.Parse(hero.GetAttribute("def"));
                heroPro.MaxExp = int.Parse(hero.GetAttribute("maxExp"));
                heroPro.Des = hero.GetAttribute("des");
                heroPro.Species = hero.GetAttribute("species");
                heroPro.Career = hero.GetAttribute("career");
                heroPro.Hp= int.Parse(hero.GetAttribute("hp"));
                heroPro.Mp= int.Parse(hero.GetAttribute("mp"));
                heroPro.Exp= int.Parse(hero.GetAttribute("exp"));
                heroPro.Organization= hero.GetAttribute("organization");
                heroPro.Money= int.Parse(hero.GetAttribute("money"));
                heroProLib.Add((int)heroPro.Type, heroPro);
            }
        }
        else
        {
            Debug.LogError("未找到文件");
        }
    }

    public Character_Product CreateHero(int _type)
    {
        if (heroProLib.Count > 0)
        {
            return heroProLib[_type].Clone();
        }
        else
        {
            Debug.LogError("没有找到英雄信息");
            return null;
        } 
    }
}

