using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
角色列表模型层，读取，创建，删除账户下角色信息
*/
public class CharList_Model
{
    //存放账户下的角色信息列表，在没有数据变动时可以直接读取数据，不需要每次访问数据库
    public List<Character_Product> characterList;
    protected CharList_Model() { }

    public static readonly CharList_Model Instance = new CharList_Model();

    //读取账户Id下的角色列表并保存，返回列表总数
    public int LoadCharacterByAccountId(string _accountId)
    {
        if (!File.Exists(AppConst.characterInfoXml))
        {
            Debug.LogError("无法加载，未找到角色信息文件");
            return 0;
        }
        else
        {
            characterList = new List<Character_Product>();
            XmlDocument doc = new XmlDocument();
            doc.Load(AppConst.characterInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            foreach (XmlElement character in root.ChildNodes)
            {
                if (character.GetAttribute("accountId") == _accountId)
                {
                    Character_Product characterPro = null;
                    Character_Product.CharType type = (Character_Product.CharType)int.Parse(character.GetAttribute("type"));
                    switch (type)
                    {
                        case Character_Product.CharType.CT_Defender:
                            //拓展
                            Defender_Product defenderPro = new Defender_Product();

                            characterPro = defenderPro;
                            break;
                        case Character_Product.CharType.CT_Melee:
                            //拓展
                            Melee_Product meleePro = new Melee_Product();

                            characterPro = meleePro;
                            break;
                        case Character_Product.CharType.CT_Archer:
                            //拓展
                            Archer_Product archerPro = new Archer_Product();

                            characterPro = archerPro;
                            break;
                        default:
                            break;
                    }
                    characterPro.Id = int.Parse(character.GetAttribute("id"));
                    characterPro.AccountId = _accountId;
                    characterPro.Type = type;
                    characterPro.TypeName = character.GetAttribute("typeName");
                    characterPro.Name = character.GetAttribute("name");
                    characterPro.Species = character.GetAttribute("species");
                    characterPro.Career = character.GetAttribute("career");
                    characterPro.Level = int.Parse(character.GetAttribute("level"));
                    characterPro.MaxHp = int.Parse(character.GetAttribute("maxHp"));
                    characterPro.MaxMp = int.Parse(character.GetAttribute("maxMp"));
                    characterPro.Atk = int.Parse(character.GetAttribute("atk"));
                    characterPro.Def = int.Parse(character.GetAttribute("def"));
                    characterPro.MaxExp = int.Parse(character.GetAttribute("maxExp"));
                    characterPro.Des = character.GetAttribute("des");
                    characterPro.Hp = int.Parse(character.GetAttribute("hp"));
                    characterPro.Mp = int.Parse(character.GetAttribute("mp"));
                    characterPro.Exp = int.Parse(character.GetAttribute("exp"));
                    characterPro.Organization = character.GetAttribute("organization");
                    characterPro.Appellation= character.GetAttribute("appellation");
                    characterPro.Money = int.Parse(character.GetAttribute("money"));
                    characterList.Add(characterPro);
                }
            }
            return characterList.Count;
        }
    }

    //创建新角色
    public void SaveCharacterInfo(string _name,int _type)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement character = doc.CreateElement("Character");
        //创建新角色
        Character_Product characterPro = Character_Factory.Instance.CreateHero(_type);
        //手动生成id
        characterPro.Id = Random.Range(10000, 99999);
        //将角色与账户和玩家名字绑定
        characterPro.Name = _name;
        characterPro.AccountId = Login_Model.Instance.accountId;
        character.SetAttribute("id", characterPro.Id.ToString());
        character.SetAttribute("accountId", characterPro.AccountId.ToString());
        character.SetAttribute("type", ((int)characterPro.Type).ToString());
        character.SetAttribute("typeName", characterPro.TypeName);
        character.SetAttribute("name", characterPro.Name);
        character.SetAttribute("level", characterPro.Level.ToString());
        character.SetAttribute("hp", characterPro.Hp.ToString());
        character.SetAttribute("maxHp", characterPro.MaxHp.ToString());
        character.SetAttribute("mp", characterPro.Mp.ToString());
        character.SetAttribute("maxMp", characterPro.MaxMp.ToString());
        character.SetAttribute("atk", characterPro.Atk.ToString());
        character.SetAttribute("def", characterPro.Def.ToString());
        character.SetAttribute("exp", characterPro.Exp.ToString());
        character.SetAttribute("maxExp", characterPro.MaxExp.ToString());
        character.SetAttribute("des", characterPro.Des);
        character.SetAttribute("species", characterPro.Species);
        character.SetAttribute("career", characterPro.Career);
        character.SetAttribute("organization", characterPro.Organization);
        character.SetAttribute("money", characterPro.Money.ToString());
        if (File.Exists(AppConst.characterInfoXml))
        {
            doc.Load(AppConst.characterInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            root.AppendChild(character);
        }
        else
        {
            XmlElement root = doc.CreateElement("Root");
            doc.AppendChild(root);
            root.AppendChild(character);
        }
        //保存到数据库
        doc.Save(AppConst.characterInfoXml);
        //保存到Model数组
        characterList.Add(characterPro);
        AddBagFile(characterPro.Id);
        AddEquipFile(characterPro.Id);
    }

    public void RemoveCharacterInfo(Character_Product _characterPro)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(AppConst.characterInfoXml);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement character in root)
        {
            if (int.Parse(character.GetAttribute("id")) == _characterPro.Id)
            { 
                root.RemoveChild(character);
                RemoveBagFile(_characterPro.Id);
                RemoveEquipFile(_characterPro.Id);
            }
        }
        //删除数据库信息
        doc.Save(AppConst.characterInfoXml);
        //删除Model中的数据
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].Id == _characterPro.Id)
            {
                characterList.Remove(characterList[i]);
            }
        }
    }

    public bool NameExists(string _name)
    {
        if (!File.Exists(AppConst.characterInfoXml))
        {
            return false;
        }
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppConst.characterInfoXml);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            foreach (XmlElement account in root.ChildNodes)
            {
                if (account.GetAttribute("name") == _name)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public void AddBagFile(int _charId)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(AppConst.bagInfoXml);
        XmlElement root = doc.SelectSingleNode("Root")as XmlElement;
        var character=doc.CreateElement("Character");
        character.SetAttribute("id", _charId.ToString());
        root.AppendChild(character);
        for (int i = 0; i < 10; i++)
        {
            var item = doc.CreateElement("Item");
            item.SetAttribute("id", "");
            item.SetAttribute("num", "");
            character.AppendChild(item);
        }
        doc.Save(AppConst.bagInfoXml);
    }

    public void RemoveBagFile(int _id)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(AppConst.bagInfoXml);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement character in root)
        {
            if (int.Parse(character.GetAttribute("id")) == _id)
            {
                root.RemoveChild(character);
            }
        }
        //删除数据库信息
        doc.Save(AppConst.bagInfoXml);
    }

    public void RemoveEquipFile(int _id)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(AppConst.equipInfoXml);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement character in root)
        {
            if (int.Parse(character.GetAttribute("id")) == _id)
            {
                root.RemoveChild(character);
            }
        }
        //删除数据库信息
        doc.Save(AppConst.equipInfoXml);
    }

    public void AddEquipFile(int _charId)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(AppConst.equipInfoXml);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        var character = doc.CreateElement("Character");
        character.SetAttribute("id", _charId.ToString());
        root.AppendChild(character);
        for (int i = 0; i < 10; i++)
        {
            var item = doc.CreateElement("Item");
            item.SetAttribute("equipType", "");
            item.SetAttribute("itemId", "");
            character.AppendChild(item);
        }
        doc.Save(AppConst.equipInfoXml);
    }

}

