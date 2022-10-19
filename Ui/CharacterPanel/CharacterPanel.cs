using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
/*
��ɫ��Ϣ���
*/
public class CharacterPanel : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    //�����ɫ����
    Character_Product character;
    //����ת������ƶ�����
    RectTransform rt;
    float minX, maxX, minY, maxY;
    Vector3 offset;

    //����ö������Text�����ͨ���������ʱ������ֵ
    AttributesType[] attributesTxt;
    //ͨ�����Լ��أ���ʡ��Դ
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

    #region ����
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
            Debug.LogError("δ���ص���ɫ��Ϣ");
            return;
        }
        foreach (var item in AttributesTxt)
        {
            switch (item.attributesType)
            {
                case TextType.TT_Hp:
                    item.textComponent.text = "����ֵ:" + character.Hp.ToString() + "/" + character.MaxHp;
                    break;
                case TextType.TT_Mp:
                    item.textComponent.text = "����ֵ:" + character.Mp.ToString() + "/" + character.MaxHp;
                    break;
                case TextType.TT_Atk:
                    item.textComponent.text = "������:" + character.Atk.ToString();
                    break;
                case TextType.TT_Def:
                    item.textComponent.text = "������:" + character.Def.ToString();
                    break;
                case TextType.TT_Species:
                    item.textComponent.text = "����:" + character.Species;
                    break;
                case TextType.TT_Appellation:
                    item.textComponent.text = "��ν:" + character.Appellation;
                    break;
                case TextType.TT_Organization:
                    item.textComponent.text = "��֯:" + character.Organization;
                    break;
                case TextType.TT_Exp:
                    item.textComponent.text = "����ֵ:" + character.Exp.ToString() + "/" + character.MaxExp;
                    break;

                default:
                    break;
            }
        }
    }

    //����װ����
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

    //װ����ɾ������
    public void UpdateEquipPanel(Items_Product _itemPro)
    {
        var item = FindItem(_itemPro);
        if (item != null)
        {
            DestroyImmediate(item.transform.gameObject);
            return;
        }
        Debug.Log("û��ƥ����Ʒ");
    }

    //װ������Ѱ�Ҷ���
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

