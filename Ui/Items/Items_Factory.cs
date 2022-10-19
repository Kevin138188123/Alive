using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
道具工厂,用于创建新道具对象
*/
public class Items_Factory
{
    Dictionary<int, Items_Product> itemProLib;

    Items_Factory() 
    {
        itemProLib = new Dictionary<int, Items_Product>();//不要忘记
        LoadItemsInfoXml(AppConst.itemsInfoXml);
    }
    public static readonly Items_Factory Instance = new Items_Factory();

    public void LoadItemsInfoXml(string _itemProXmlUrl)
    {
        if (!File.Exists(_itemProXmlUrl))
        {
            Debug.LogError("itemProXmlUrl未找到");
            return;
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(_itemProXmlUrl);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;

        foreach (XmlElement item in root.ChildNodes)
        {
            Items_Product itemPro =null;
            Items_Product.ItemType type=(Items_Product.ItemType)int.Parse(item.GetAttribute("type"));
            switch (type)   
            {
                case Items_Product.ItemType.None:
                    break;
                case Items_Product.ItemType.IT_Drug:
                    //药品分类，使用时可以区分
                    Drug_Product.DrugType drugType=(Drug_Product.DrugType)int.Parse(item.GetAttribute("drugType"));
                    Drug_Product drugPro = new Drug_Product();
                    switch (drugType)
                    {
                        case Drug_Product.DrugType.None:
                            break;
                        case Drug_Product.DrugType.DT_Hp:
                            drugPro.Hp = int.Parse(item.GetAttribute("hp"));
                            break;
                        case Drug_Product.DrugType.DT_Mp:
                            drugPro.Mp = int.Parse(item.GetAttribute("mp"));
                            break;
                        case Drug_Product.DrugType.DT_HpBuff:
                            drugPro.MaxHp = int.Parse(item.GetAttribute("maxHp"));
                            drugPro.BuffTime = int.Parse(item.GetAttribute("buffTime"));
                            break;
                        case Drug_Product.DrugType.DT_MpBuff:
                            drugPro.MaxMp = int.Parse(item.GetAttribute("maxMp"));
                            drugPro.BuffTime = int.Parse(item.GetAttribute("buffTime"));
                            break;
                        case Drug_Product.DrugType.DT_AtkBuff:
                            drugPro.Atk = int.Parse(item.GetAttribute("atk"));
                            drugPro.BuffTime = int.Parse(item.GetAttribute("buffTime"));
                            break;
                        case Drug_Product.DrugType.DT_DefBuff:
                            drugPro.Def = int.Parse(item.GetAttribute("def"));
                            drugPro.BuffTime = int.Parse(item.GetAttribute("buffTime"));
                            break;
                        default:
                            break;
                    }
                    drugPro.Drug_Type =(Drug_Product.DrugType)int.Parse(item.GetAttribute("drugType"));
                    itemPro = drugPro;
                    break;
                case Items_Product.ItemType.IT_Clutter:
                    Clutter_Product clutterPro = new Clutter_Product();
                    clutterPro.Clutter_Type = (Clutter_Product.ClutterType)int.Parse(item.GetAttribute("clutterType"));
                    itemPro = clutterPro;
                    break;
                case Items_Product.ItemType.IT_Equip:
                    Equip_Product equipPro = new Equip_Product();
                    equipPro.Equip_Type = (Equip_Product.EquipType)int.Parse(item.GetAttribute("equipType"));
                    equipPro.Atk = int.Parse(item.GetAttribute("atk"));
                    equipPro.Def = int.Parse(item.GetAttribute("def"));
                    equipPro.MaxHp = int.Parse(item.GetAttribute("maxHp"));
                    equipPro.MaxMp = int.Parse(item.GetAttribute("maxMp"));
                    itemPro = equipPro;
                    break;
                default:
                    break;
            }
            itemPro.Id = int.Parse(item.GetAttribute("id"));
            itemPro.Item_Type = type;
            itemPro.ItemName = item.GetAttribute("itemName");
            itemPro.TypeName = item.GetAttribute("typeName");
            itemPro.BuyPrice = int.Parse(item.GetAttribute("buyPrice"));
            itemPro.SellPrice = int.Parse(item.GetAttribute("sellPrice"));
            itemPro.IconPath = item.GetAttribute("iconPath");
            itemPro.MaxNum = int.Parse(item.GetAttribute("maxNum"));
            itemPro.Num = int.Parse(item.GetAttribute("num")); ;
            itemPro.Desc = item.GetAttribute("desc");
            itemProLib.Add(itemPro.Id,itemPro);
        }
    }

    //return itemProLib[_id]会直接拿到内存中已有的对象，在对比Item是否相同时
    //他们的内存地址相同，所以判断相同，造成背包系统判断同物品时出错
    //这里我们用克隆函数赋值了一份拷贝数据
    public Items_Product CreateItemById(int _id)
    {
        if (itemProLib.ContainsKey(_id))
        { 
            return itemProLib[_id].Clone();
        }
        Debug.Log("无法创建物品");
        return null;
    }


}

