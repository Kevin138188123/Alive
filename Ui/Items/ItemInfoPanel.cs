using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
��Ʒ�������
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
        //��ΪCanvas���Ӷ��������ǰ��
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
        
        //�������ĵ㣨areaPanel��BagPanel�Ĺ�����̬���ԣ��������أ�
        Vector2 pointer = BagPanel.Instance.areaPanel.GetPointInArea(Input.mousePosition);
        //RectTransform�����ص�
        transform.GetComponent<RectTransform>().pivot = pointer;
        //��������
        transform.position = _transform.position;

        nameTxt.text = _itemPro.ItemName;
        typeNameTxt.text = _itemPro.TypeName;
        idTxt.text = "ID:" + _itemPro.Id.ToString();
        despTxt.text = "����:" + _itemPro.Desc;
        sellPriceTxt.text = "����:" + _itemPro.SellPrice.ToString();
        buyPriceTxt.text = "����:" + _itemPro.BuyPrice.ToString();
        icon.sprite = Resources.Load<Sprite>(_itemPro.IconPath);
        transform.tag = "ItemInfoPanel";
    }
}

