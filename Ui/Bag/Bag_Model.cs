using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
����ģ�Ͳ㣺��ȡ���洢���޸Ľ�ɫ������װ�����еĵ���
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

    //�����������ʱ���ã���ȡ����
    public void LoadBagInfo()
    {
        if (!File.Exists(bagInfoXmlUrl))
        {
            Debug.LogError("BagInfoXmlδ�ҵ�");
        }
        XmlDocument doc = new XmlDocument();
        doc.Load(bagInfoXmlUrl);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement charId in root.ChildNodes)
        {
            if (int.Parse(charId.GetAttribute("id")) == AppConst.charId)//ƥ���ɫ
            {
                foreach (XmlElement item in charId.ChildNodes)//��ȡ��ɫ�ı�������
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
                        //����ձ���������(��������)
                        bagItemsList.Add(null);
                        BagPanel.Instance.CreateGridItem();//����Ʒ��grid
                    }
                }

            }
        }
    }

    //��ӵ��ߣ�ʵʱ�������ݿ�
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
                    //��ǰ����+��ֵ=�������
                    itemPro.Num += deviation;
                    //���±����еĵ�ǰ��Ʒ����
                    BagPanel.Instance.UpdateItemCount(itemPro);//��������߶���Init()�����UpdateItemCount()
                    //����ӵ�����-��ֵ=ʣ���������������
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
        SaveBagItem();//��Ӻ�ִ�б���
    }

    //���������ݱ��������ݿ�
    public void SaveBagItem()
    {
        var grids = BagPanel.Instance.Grids;

        XmlDocument doc = new XmlDocument();
        doc.Load(bagInfoXmlUrl);
        XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
        foreach (XmlElement charId in root.ChildNodes)
        {
            if (int.Parse(charId.GetAttribute("id")) == AppConst.charId)//ƥ���ɫ
            {
                charId.RemoveAll();//ɾ�����ɾ������ǩ����������
                charId.SetAttribute("id", AppConst.charId.ToString());//������ӽ�ɫID
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

    //ʹ����Ʒ
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
    //                //�������������ִ�и�������
    //                BagPanel.Instance.UpdateItemCount(item);
    //                return;
    //            }
    //            else
    //            {
    //                var deviation = item.MaxNum - item.Num;
    //                item.Num += deviation;
    //                //������������ȸ����ۼƵ����ֵ��item����������֮���½�
    //                BagPanel.Instance.UpdateItemCount(item);
    //                newNum = _num - deviation;
    //                break;
    //            }
    //        }
    //    }
    //    //�µ��ߵĴ���
    //    Items_Product itemPro = Items_Factory.Instance.CreateItemById(_id);
    //    itemPro.Num = _num;
    //    //������Χ�Ĵ���(��Ϊ0˵���Ǵ����ҳ�����Χ�ĵ���)
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
    //                //�������������ִ�и�������
    //                BagPanel.Instance.UpdateItemCount(item);
    //                return;
    //            }
    //            else
    //            {
    //                var deviation = item.MaxNum - item.Num;
    //                item.Num += deviation;
    //                //������������ȸ����ۼƵ����ֵ��item����������֮���½�
    //                BagPanel.Instance.UpdateItemCount(item);
    //                newNum = _num - deviation;
    //                break;
    //            }
    //        }
    //    }
    //    //�µ��ߵĴ���
    //    Items_Product itemPro = Items_Factory.Instance.CreateItemById(_id);
    //    itemPro.Num = _num;
    //    //������Χ�Ĵ���(��Ϊ0˵���Ǵ����ҳ�����Χ�ĵ���)
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
    //    foreach (XmlElement charItem in root.ChildNodes)//������Ŀ¼
    //    {
    //        if (int.Parse(charItem.GetAttribute("id")) == AppConst.charId)//ƥ���ɫId
    //        {
    //            int newNum = 0;
    //            bool isUpdate = false;
    //            foreach (XmlElement xmlItem in charItem.ChildNodes)//������ɫid�µ�����
    //            {

    //                if (xmlItem.GetAttribute("itemId") == _id + "")//�ҵ���Ʒ
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
    //                        //�������ݿ�
    //                        xmlItem.SetAttribute("num", curNum.ToString());//�޸ĵ�������
    //                        doc.Save(bagInfoXmlUrl);
    //                        //����UI
    //                        itemPro.Num = curNum;
    //                        BagPanel.Instance.UpdateItemCount(itemPro);
    //                        isUpdate = true;
    //                        return;
    //                    }
    //                    else
    //                    {
    //                        var deviation = itemPro.MaxNum - curNum;
    //                        curNum += deviation;
    //                        //�������ݿ�
    //                        xmlItem.SetAttribute("num", curNum.ToString());//�޸ĵ�������
    //                        doc.Save(bagInfoXmlUrl);
    //                        //����UI
    //                        itemPro.Num = curNum;
    //                        BagPanel.Instance.UpdateItemCount(itemPro);
    //                        newNum = _num - deviation;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (!isUpdate)
    //            {
    //                foreach (XmlElement xmlItem in charItem.ChildNodes)//������ɫid�µ�����
    //                {
    //                    if (xmlItem.GetAttribute("itemId") == "")
    //                    {
    //                        //���ݿⲻ����Ϊ�µ��ߵĴ���
    //                        Items_Product newItemPro = Items_Factory.Instance.CreateItemById(_id);
    //                        newItemPro.Num = _num;
    //                        //������Χ�Ĵ���(��Ϊ0˵���Ǵ����ҳ�����Χ�ĵ���)
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

