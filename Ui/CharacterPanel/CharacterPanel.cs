using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/*
角色信息面板
*/
public class CharacterPanel : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    //保存角色对象
    Character_Product character;
    //坐标转换面板移动限制
    RectTransform rt;
    float minX, maxX, minY, maxY;
    Vector3 offset;

    //利用枚举区分Text组件，通过数组加载时遍历赋值
    AttributesType[] attributesTxt;
    //通过属性加载，节省资源
    public AttributesType[] AttributesTxt
    {
        get
        {
            if (attributesTxt == null)
            {
                attributesTxt = transform.Find("AttributePanel").GetComponentsInChildren<AttributesType>();
            }
            return attributesTxt;
        }
    }

    AttributesType[] titleTxt;
    public AttributesType[] TitleTxt
    {
        get
        {
            if (titleTxt == null)
            {
                titleTxt = transform.Find("TitlePanel").GetComponentsInChildren<AttributesType>();
            }
            return titleTxt;
        }
    }

    Equip_Grid[] equipGrids;
    public Equip_Grid[] EquipGrids
    {
        get
        {
            if (equipGrids == null)
            {
                return equipGrids = transform.Find("EquipPanel").GetComponentsInChildren<Equip_Grid>();
            }
            return equipGrids;
        }
    }

    #region 单例
    CharacterPanel() { }
    static CharacterPanel instance;
    static readonly object locker = new object();
    public static CharacterPanel Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new CharacterPanel();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        instance = this;
        rt = transform.GetComponent<RectTransform>();
        character = Character_Model.Instancce.character;
        UpdateAttributesPanel();
        UpdateTitlePanel();
        Equip_Model.Instance.loadEquipInfo();
        MessageEventSystem.Instance.AddListener(EventMessage.hpUpdateMes,UpdateAttributesPanel);
    }

    public void UpdateTitlePanel()
    {
        foreach (var item in TitleTxt)
        {
            switch (item.attributesType)
            {
                case TextType.TT_Name:
                    item.textComponent.text = character.Name;
                    break;
                case TextType.TT_Career:
                    item.textComponent.text = character.Career;
                    break;
                case TextType.TT_Level:
                    item.textComponent.text = "Lv:" + character.Level.ToString();
                    break;
                default:
                    break;
            }
        }
    }

    private void UpdateAttributesPanel()
    {
        if (character == null)
        {
            Debug.LogError("未加载到角色信息");
            return;
        }
        foreach (var item in AttributesTxt)
        {
            switch (item.attributesType)
            {
                case TextType.TT_Hp:
                    item.textComponent.text = "生命值:" + character.Hp.ToString() + "/" + character.MaxHp;
                    break;
                case TextType.TT_Mp:
                    item.textComponent.text = "活力值:" + character.Mp.ToString() + "/" + character.MaxHp;
                    break;
                case TextType.TT_Atk:
                    item.textComponent.text = "攻击力:" + character.Atk.ToString();
                    break;
                case TextType.TT_Def:
                    item.textComponent.text = "防御力:" + character.Def.ToString();
                    break;
                case TextType.TT_Species:
                    item.textComponent.text = "种族:" + character.Species;
                    break;
                case TextType.TT_Appellation:
                    item.textComponent.text = "称谓:" + character.Appellation;
                    break;
                case TextType.TT_Organization:
                    item.textComponent.text = "组织:" + character.Organization;
                    break;
                case TextType.TT_Exp:
                    item.textComponent.text = "经验值:" + character.Exp.ToString() + "/" + character.MaxExp;
                    break;

                default:
                    break;
            }
        }
    }

    //加载装备栏
    public void CreateEquipItem(Equip_Product _equipInfo)
    {
        var item = GameObject.Instantiate(Resources.Load<Item>("Prefabs/Item"));
        foreach (var grid in EquipGrids)
        {
            if (_equipInfo.Equip_Type == grid.equipType)
            {
                item.Init(_equipInfo, grid.transform);
            }
        }
    }

    //装备栏删除更新
    public void UpdateEquipPanel(Items_Product _itemPro)
    {
        var item = FindItem(_itemPro);
        if (item != null)
        {
            DestroyImmediate(item.transform.gameObject);
            return;
        }
        Debug.Log("没有匹配物品");
    }

    //装备栏中寻找对象
    public Item FindItem(Items_Product _itemPro)
    {
        for (int i = 0; i < EquipGrids.Length; i++)
        {
            if (EquipGrids[i].transform.childCount == 2)
            {
                var item = EquipGrids[i].transform.GetChild(1).GetComponent<Item>();
                if (item.ItemPro == _itemPro)
                {
                    return item;
                }
            }
        }
        return null;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out Vector3 worldMousePos))
        {
            offset = rt.position - worldMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out Vector3 worldMousePos))
        {
            rt.position = SetDragRange(offset + worldMousePos);
        }
    }

    Vector3 SetDragRange(Vector3 pos)
    {
        minX = rt.rect.width * rt.pivot.x;
        maxX = Screen.width - rt.rect.width * (1 - rt.pivot.x);
        minY = rt.rect.height * rt.pivot.y;
        maxY = Screen.height - rt.rect.height * (1 - rt.pivot.y);
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }

}

