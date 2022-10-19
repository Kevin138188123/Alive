using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
物品详情面板
*/
public class ItemInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI nameTxt;
    public TextMeshProUGUI typeNameTxt;
    public TextMeshProUGUI idTxt;
    public TextMeshProUGUI despTxt;
    public TextMeshProUGUI sellPriceTxt;
    public TextMeshProUGUI buyPriceTxt;
    public Image icon;

    public void Init(Items_Product _itemPro, Transform _transform)
    {
        //作为Canvas的子对象放在最前层
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        
        //设置中心点（areaPanel是BagPanel的公共静态属性，面板会隐藏）
        Vector2 pointer = BagPanel.Instance.areaPanel.GetPointInArea(Input.mousePosition);
        //RectTransform设置重点
        transform.GetComponent<RectTransform>().pivot = pointer;
        //设置坐标
        transform.position = _transform.position;

        nameTxt.text = _itemPro.ItemName;
        typeNameTxt.text = _itemPro.TypeName;
        idTxt.text = "ID:" + _itemPro.Id.ToString();
        despTxt.text = "详情:" + _itemPro.Desc;
        sellPriceTxt.text = "出售:" + _itemPro.SellPrice.ToString();
        buyPriceTxt.text = "购买:" + _itemPro.BuyPrice.ToString();
        icon.sprite = Resources.Load<Sprite>(_itemPro.IconPath);
        transform.tag = "ItemInfoPanel";
    }
}

