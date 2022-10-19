using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
道具模型层：读取、存储和修改角色背包、装备栏中的道具
*/
public class Bag_Model
{
    public List<Items_Product> bagItemsList;
    string bagInfoXmlUrl;
    Bag_Model()
    {
        bagItemsList = new List<Items_Product>();
        bagInfoXmlUrl = AppConst.bagInfoXml;
    }
    public static readonly Bag_Model Instance = new Bag_Model();

    //背包组件加载时调用，读取数据
    public void LoadBagInfo()
    {
        if (!File.Exists(bagInfoXmlUrl))
        {
            Debug.LogError("BagInfoXml未找到");
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(bagInfoXmlUrl);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement charId in root.ChildNodes)
        {
            if (int.Parse(charId.GetAttribute("id")) == AppConst.charId)//匹配角色
            {
                foreach (XmlElement item in charId.ChildNodes)//读取角色的背包数据
                {
                    if (item.GetAttribute("id") != "")
                    {
                        int itemId = int.Parse(item.GetAttribute("id"));
                        Items_Product itemPro = Items_Factory.Instance.CreateItemById(itemId);
                        itemPro.Num = int.Parse(item.GetAttribute("num"));
                        BagPanel.Instance.CreateGridItem(itemPro);
                        bagItemsList.Add(itemPro);
                    }
                    else
                    {
                        //加入空保留空网格(背包容量)
                        bagItemsList.Add(null);
                        BagPanel.Instance.CreateGridItem();//无物品加grid
                    }
                }

            }
        }
    }

    //添加道具，实时更新数据库
    public void AddItem(int _id, int _count)
    {
        var newCount = 0;
        foreach (var itemPro in bagItemsList)
        {
            if (itemPro.Id == _id)
            {
                if (itemPro.Num == 99)
                {
                    continue;
                }
                if (itemPro.Num + _count <= itemPro.MaxNum)
                {
                    itemPro.Num += _count;
                    BagPanel.Instance.UpdateItemCount(itemPro);
                    return;
                }
                else
                {
                    var deviation = itemPro.MaxNum - itemPro.Num;
                    //当前数量+差值=最大数量
                    itemPro.Num += deviation;
                    //更新背包中的当前物品数量
                    BagPanel.Instance.UpdateItemCount(itemPro);//不进入道具对象Init()需调用UpdateItemCount()
                    //将添加的数量-差值=剩余的数量用于新增
                    newCount = _count - deviation;
                    break;
                }
            }
        }
        Items_Product item = Items_Factory.Instance.CreateItemById(_id);
        item.Num = _count;
        if (newCount != 0)
        {
            item.Num = newCount;
        }
        bagItemsList.Add(item);
        BagPanel.Instance.UpdateBagPanel(item, false);
        newCount = 0;
        SaveBagItem();//添加后执行保存
    }

    //将集合数据保存至数据库
    public void SaveBagItem()
    {
        var grids = BagPanel.Instance.Grids;

        XmlDocument doc = new XmlDocument();
        doc.Load(bagInfoXmlUrl);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement charId in root.ChildNodes)
        {
            if (int.Parse(charId.GetAttribute("id")) == AppConst.charId)//匹配角色
            {
                charId.RemoveAll();//删除子项并删除本标签的所有属性
                charId.SetAttribute("id", AppConst.charId.ToString());//重新添加角色ID
                foreach (var grid in grids)
                {
                    var item = doc.CreateElement("Item");
                    if (grid.transform.childCount != 0)
                    {
                        var itemPro = grid.transform.GetChild(0).GetComponent<Item>().ItemPro;
                        item.SetAttribute("id", itemPro.Id.ToString());
                        item.SetAttribute("num", itemPro.Num.ToString());
                    }
                    else
                    {
                        item.SetAttribute("id", "");
                        item.SetAttribute("num", "");
                    }
                    charId.AppendChild(item);
                }
                doc.Save(bagInfoXmlUrl);
            }
            return;
        }
    }

    //使用物品
    public void DropItem(Items_Product _item, bool _isUse = false)
    {
        if (_isUse)
        {
            if (_item.Num - 1 <= 0)
            {
                DropListItem(_item);
                BagPanel.Instance.UpdateBagPanel(_item, true);
            }
            else
            {
                _item.Num -= 1;
                BagPanel.Instance.UpdateItemCount(_item);
            }
        }
        else
        {
            DropListItem(_item);
            BagPanel.Instance.UpdateBagPanel(_item, true);
        }
        SaveBagItem();
    }

    public void UpdateBagList(Items_Product _itemPro, bool isAdd = false)
    {
        if (isAdd)
        {
            bagItemsList.Add(_itemPro);
        }
        else
        {
            bagItemsList.Remove(_itemPro);
        }

    }

    public void DropListItem(Items_Product _itemPro)
    {
        for (int i = 0; i < bagItemsList.Count; i++)
        {
            if (bagItemsList[i] == _itemPro)
            {
                bagItemsList[i] = null;
            }
        }
    }

    public void AddListItem(Items_Product _itemPro)
    {
        for (int i = 0; i < bagItemsList.Count; i++)
        {
            if (bagItemsList[i] == null)
            {
                bagItemsList[i] = _itemPro;
            }
        }
    }

    #region COPY
    //public void AddItem(int _id, int _num)
    //{
    //    int newNum = 0;
    //    foreach (var item in bagItemsList)
    //    {
    //        if (item.Id == _id)
    //        {
    //            if (item.Num == 99)
    //            {
    //                continue;
    //            }
    //            if (item.Num + _num <= item.MaxNum)
    //            {
    //                item.Num += _num;
    //                //不超出最大数量执行更新数量
    //                BagPanel.Instance.UpdateItemCount(item);
    //                return;
    //            }
    //            else
    //            {
    //                var deviation = item.MaxNum - item.Num;
    //                item.Num += deviation;
    //                //超出最大数量先更新累计到最大值的item，保留余数之后新建
    //                BagPanel.Instance.UpdateItemCount(item);
    //                newNum = _num - deviation;
    //                break;
    //            }
    //        }
    //    }
    //    //新道具的创建
    //    Items_Product itemPro = Items_Factory.Instance.CreateItemById(_id);
    //    itemPro.Num = _num;
    //    //超出范围的创建(不为0说明是存在且超出范围的道具)
    //    if (newNum != 0)
    //    {
    //        itemPro.Num = newNum;
    //    }
    //    bagItemsList.Add(itemPro);
    //    BagPanel.Instance.UpdateBagPanel(itemPro, false);
    //    newNum = 0;
    //}    //public void AddItem(int _id, int _num)
    //{
    //    int newNum = 0;
    //    foreach (var item in bagItemsList)
    //    {
    //        if (item.Id == _id)
    //        {
    //            if (item.Num == 99)
    //            {
    //                continue;
    //            }
    //            if (item.Num + _num <= item.MaxNum)
    //            {
    //                item.Num += _num;
    //                //不超出最大数量执行更新数量
    //                BagPanel.Instance.UpdateItemCount(item);
    //                return;
    //            }
    //            else
    //            {
    //                var deviation = item.MaxNum - item.Num;
    //                item.Num += deviation;
    //                //超出最大数量先更新累计到最大值的item，保留余数之后新建
    //                BagPanel.Instance.UpdateItemCount(item);
    //                newNum = _num - deviation;
    //                break;
    //            }
    //        }
    //    }
    //    //新道具的创建
    //    Items_Product itemPro = Items_Factory.Instance.CreateItemById(_id);
    //    itemPro.Num = _num;
    //    //超出范围的创建(不为0说明是存在且超出范围的道具)
    //    if (newNum != 0)
    //    {
    //        itemPro.Num = newNum;
    //    }
    //    bagItemsList.Add(itemPro);
    //    BagPanel.Instance.UpdateBagPanel(itemPro, false);
    //    newNum = 0;
    //}

    //public void AddItem(int _id, int _num)
    //{
    //    XmlDocument doc = new XmlDocument();
    //    doc.Load(bagInfoXmlUrl);
    //    XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
    //    foreach (XmlElement charItem in root.ChildNodes)//遍历根目录
    //    {
    //        if (int.Parse(charItem.GetAttribute("id")) == AppConst.charId)//匹配角色Id
    //        {
    //            int newNum = 0;
    //            bool isUpdate = false;
    //            foreach (XmlElement xmlItem in charItem.ChildNodes)//遍历角色id下的网格
    //            {

    //                if (xmlItem.GetAttribute("itemId") == _id + "")//找到物品
    //                {
    //                    var itemPro = Items_Factory.Instance.CreateItemById(_id);
    //                    var curNum = int.Parse(xmlItem.GetAttribute("num"));
    //                    if (curNum == 99)
    //                    {
    //                        continue;
    //                    }
    //                    if (curNum + _num <= itemPro.MaxNum)
    //                    {
    //                        curNum += _num;
    //                        //更新数据库
    //                        xmlItem.SetAttribute("num", curNum.ToString());//修改道具数量
    //                        doc.Save(bagInfoXmlUrl);
    //                        //更新UI
    //                        itemPro.Num = curNum;
    //                        BagPanel.Instance.UpdateItemCount(itemPro);
    //                        isUpdate = true;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        var deviation = itemPro.MaxNum - curNum;
    //                        curNum += deviation;
    //                        //更新数据库
    //                        xmlItem.SetAttribute("num", curNum.ToString());//修改道具数量
    //                        doc.Save(bagInfoXmlUrl);
    //                        //更新UI
    //                        itemPro.Num = curNum;
    //                        BagPanel.Instance.UpdateItemCount(itemPro);
    //                        newNum = _num - deviation;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (!isUpdate)
    //            {
    //                foreach (XmlElement xmlItem in charItem.ChildNodes)//遍历角色id下的网格
    //                {
    //                    if (xmlItem.GetAttribute("itemId") == "")
    //                    {
    //                        //数据库不存在为新道具的创建
    //                        Items_Product newItemPro = Items_Factory.Instance.CreateItemById(_id);
    //                        newItemPro.Num = _num;
    //                        //超出范围的创建(不为0说明是存在且超出范围的道具)
    //                        if (newNum != 0)
    //                        {
    //                            newItemPro.Num = newNum;
    //                        }
    //                        xmlItem.SetAttribute("itemId", _id.ToString());
    //                        xmlItem.SetAttribute("num", newItemPro.Num.ToString());
    //                        doc.Save(bagInfoXmlUrl);
    //                        BagPanel.Instance.UpdateBagPanel(newItemPro, false);
    //                        newNum = 0;
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    #endregion
}

