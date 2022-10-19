using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
背包分页管理控制
*/
public class TogglePanel : MonoBehaviour
{
    public Toggle[] toggleGroup;
    public Transform bag;
    public Transform otherBag;
    List<Items_Product> otherItemList;
    Items_Product.ItemType type;

    private void Start()
    {
        otherItemList = new List<Items_Product>();
        foreach (var item in toggleGroup)
        {
            item.onValueChanged.AddListener(OnValueChanged);
        }
    }

    void OnValueChanged(bool isOn)
    {
        if (isOn)
        {
            for (int i = 0; i < toggleGroup.Length; i++)
            {
                //辨别所选项
                if (toggleGroup[i].isOn)
                {
                    type = toggleGroup[i].transform.GetComponent<BagToggle>().type;
                    break;
                }
            }
            if (type == Items_Product.ItemType.None)
            {
                bag.localScale = Vector3.one;
                otherBag.localScale = Vector3.zero;
            }
            else
            {
                bag.localScale = Vector3.zero;
                otherBag.localScale = Vector3.one;
                FindItemByType();
                UpdateOtherBag();
            }
        }
        else
        {
            Debug.Log(false);
        }
    }

    public void UpdateOtherBag()
    {
        foreach (var itemPro in otherItemList)
        {
            var grid = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bag_Grid"));
            grid.transform.SetParent(otherBag);
            var item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
            item.transform.GetComponent<Item>().Init(itemPro, grid.transform);
        }

    }

    public void FindItemByType()
    {
        var grids = otherBag.GetComponentsInChildren<Bag_Grid>();
        foreach (var item in grids)
        {
            Debug.Log(item.gameObject);
            Destroy(item.gameObject);
           
        }

        foreach (var itemPro in Bag_Model.Instance.bagItemsList)
        {
            if (itemPro != null)
            {
                if (itemPro.Item_Type == type)
                {
                    otherItemList.Add(itemPro);
                }
            }
        }
    }

    //void Test()
    //{
    //    foreach (var itemPro in Bag_Model.Instance.bagItemsList)
    //    {
    //        if (itemPro != null)
    //        {
    //            if (BagPanel.Instance.FindItem(itemPro) == null)
    //            {
    //                for (int i = 0; i < bagGrids.Length; i++)
    //                {
    //                    if (bagGrids[i].transform.childCount == 0)
    //                    {
    //                        var item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
    //                        item.transform.GetComponent<Item>().Init(itemPro, bagGrids[i].transform);
    //                        break;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}



}

