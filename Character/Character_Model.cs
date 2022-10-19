using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
角色模型层:当前游戏角色数据的读取，修改
*/
public class Character_Model
{
    public Character_Product character;
    string charInfoXml;
    int charId;
    Character_Model()
    {
        charId = AppConst.charId;
        charInfoXml = AppConst.characterInfoXml;
        InitXml(charInfoXml, charId);
    }

    public static readonly Character_Model Instancce = new Character_Model();

    //进入游戏加载角色信息（通过角色选择界面保留在AppConst中的AccountId）
    void InitXml(string _fileName, int _id)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(_fileName);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement charInfo in root.ChildNodes)
        {
            if (int.Parse(charInfo.GetAttribute("id")).Equals(_id))
            {
                //角色有不同属性时需要辨别角色类型
                character = new Character_Product();
                character.Id = int.Parse(charInfo.GetAttribute("id"));
                character.AccountId = charInfo.GetAttribute("accountId"); ;
                character.Type = (Character_Product.CharType)int.Parse(charInfo.GetAttribute("type"));
                character.TypeName = charInfo.GetAttribute("typeName");
                character.Name = charInfo.GetAttribute("name");
                character.Species = charInfo.GetAttribute("species");
                character.Career = charInfo.GetAttribute("career");
                character.Level = int.Parse(charInfo.GetAttribute("level"));
                character.MaxHp = int.Parse(charInfo.GetAttribute("maxHp"));
                character.MaxMp = int.Parse(charInfo.GetAttribute("maxMp"));
                character.Atk = int.Parse(charInfo.GetAttribute("atk"));
                character.Def = int.Parse(charInfo.GetAttribute("def"));
                character.MaxExp = int.Parse(charInfo.GetAttribute("maxExp"));
                character.Des = charInfo.GetAttribute("des");
                character.Hp = int.Parse(charInfo.GetAttribute("hp"));
                character.Mp = int.Parse(charInfo.GetAttribute("mp"));
                character.Exp = int.Parse(charInfo.GetAttribute("exp"));
                character.Organization = charInfo.GetAttribute("organization");
                character.Appellation= charInfo.GetAttribute("appellation");
                character.Money = int.Parse(charInfo.GetAttribute("money"));
            }
        }
    }

    public void UseDrug(Drug_Product _itemPro)
    {
        switch (_itemPro.Drug_Type)
        {
            case Drug_Product.DrugType.None:
                break;
            case Drug_Product.DrugType.DT_Hp:
                character.Hp += _itemPro.Hp;
                character.Hp = character.Hp > character.MaxHp ? character.MaxHp : character.Hp;
                break;
            case Drug_Product.DrugType.DT_Mp:
                character.Mp += _itemPro.Mp;
                character.Mp = character.Mp > character.MaxMp ? character.MaxMp : character.Mp;
                break;
            case Drug_Product.DrugType.DT_HpBuff:
                character.MaxHp += _itemPro.MaxHp;
                break;
            case Drug_Product.DrugType.DT_MpBuff:
                character.MaxMp += _itemPro.MaxMp;
                break;
            case Drug_Product.DrugType.DT_AtkBuff:
                character.Atk += _itemPro.Atk;
                break;
            case Drug_Product.DrugType.DT_DefBuff:
                character.Def += _itemPro.Def;
                break;
            default:
                break;
        }
    }

    public void AddEquip(Equip_Product _itemPro)
    {
        character.Atk += _itemPro.Atk;
        character.Def += _itemPro.Def;
        character.MaxHp += _itemPro.MaxHp;
        character.MaxMp += _itemPro.MaxMp;
        Equip_Model.Instance.equipItemList.Add(_itemPro);
    }

    public void DropEquip(Equip_Product _itemPro)
    {
        character.Atk -= _itemPro.Atk;
        character.Def -= _itemPro.Def;
        character.MaxHp -= _itemPro.MaxHp;
        character.MaxMp -= _itemPro.MaxMp;
        Equip_Model.Instance.equipItemList.Remove(_itemPro);
    }

    public void Hurt()
    {
        character.Hp -= 5;
        character.Hp = character.Hp < 0 ? 0 : character.Hp;
        Death();
    }

    public void Attack()
    { 
        
    }

    public void Death()
    {
        if (Character_Model.Instancce.character.Hp <= 0)
        {
            if (!SubmitPanel.Instance.transform.parent.gameObject.activeSelf)
            {
                SubmitPanel.Instance.titleTxt.text = "复活";
                SubmitPanel.Instance.infoTxt.text = character.Name;
                SubmitPanel.Instance.type = SubmitType.death;
                SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
            }
        }
    }
}

