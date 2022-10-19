using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
/*
装备模型层
*/
public class Equip_Model
{
    public List<Equip_Product> equipItemList;
    string xmlUrl;
    Equip_Model()
    {
        equipItemList = new List<Equip_Product>();
        xmlUrl = AppConst.equipInfoXml;
    }
    public static readonly Equip_Model Instance = new Equip_Model();

    public void loadEquipInfo()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlUrl);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement character in root.ChildNodes)
        {
            if (int.Parse(character.GetAttribute("id")) == AppConst.charId)
            {
                foreach (XmlElement item in character.ChildNodes)
                {
                    if (item.GetAttribute("itemId") != "")
                    {
                        var itemId = int.Parse(item.GetAttribute("itemId"));
                        Equip_Product equipPro = Items_Factory.Instance.CreateItemById(itemId) as Equip_Product;
                        CharacterPanel.Instance.CreateEquipItem(equipPro);
                        equipItemList.Add(equipPro);
                    }
                }
            }
        }
    }

    public void SaveEquipItem()
    {
        var grids = CharacterPanel.Instance.EquipGrids;
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlUrl);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement character in root.ChildNodes)
        {
            if (int.Parse(character.GetAttribute("id")) == AppConst.charId)
            {
                character.RemoveAll();
                character.SetAttribute("id", AppConst.charId.ToString());
                foreach (var grid in grids)
                {
                    var item = doc.CreateElement("Item");
                    if (grid.transform.childCount != 1)
                    {
                        var equipItem = grid.transform.GetChild(1).GetComponent<Item>().ItemPro as Equip_Product;
                        item.SetAttribute("equipType", ((int)equipItem.Equip_Type).ToString());
                        item.SetAttribute("itemId", equipItem.Id.ToString());
                    }
                    else
                    {
                        item.SetAttribute("equipType", "");
                        item.SetAttribute("itemId", "");
                    }
                    character.AppendChild(item);
                }
                doc.Save(xmlUrl);
            }
            return;
        }
    }

    public void DropEquip(Items_Product _item)
    {
        //移除列表
        equipItemList.Remove(_item as Equip_Product);
        //显示更新
        CharacterPanel.Instance.UpdateEquipPanel(_item);
        //保存数据
        SaveEquipItem();
    }


}

