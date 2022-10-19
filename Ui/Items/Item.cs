using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
��Ʒ�������
*/
public class Item : MonoBehaviour
{
    Items_Product itemPro;//���ڳ�ʼ��Itemʱ���ն�������������Ҫ�õ�
    public Items_Product ItemPro
    {
        get { return itemPro; }
    }

    Image image;
    Sprite sprite;
    TextMeshProUGUI numTxt;
    ItemInfoPanel itemInfoPrefab;//Transform��Gameobject���Ͳ�����ʱ��ʹ����������
    ItemInfoPanel itemInfoPanel;

    Transform rawParent;

    private void Awake()
    {
        image = GetComponent<Image>();
        sprite = GetComponent<Sprite>();
        numTxt = transform.Find("NumTxt").GetComponent<TextMeshProUGUI>();
        itemInfoPrefab = Resources.Load<ItemInfoPanel>("Prefabs/ItemInfoPanel");
    }

    public void Init(Items_Product _itemPro, Transform _parent)
    {
        itemPro = _itemPro;
        sprite = Resources.Load<Sprite>(itemPro.IconPath);
        image.sprite = sprite;
        numTxt.text = _itemPro.Num.ToString();
        transform.SetParent(_parent, false);
        transform.localPosition = Vector3.zero;
        UpdateItemCount();
    }

    //��ǰ������Model��List�еĶ���ͬһ���ڴ��ַ����MOdel���޸ĺ�ֱ���޸����ڴ��е�����
    public void UpdateItemCount()
    {
        if (itemPro.Num > 1)
        {
            numTxt.text = itemPro.Num.ToString();
        }
        else
        {
            numTxt.text = "";
        }

    }
    /// <summary>
    /// ��Ʒ����
    /// </summary>
    /// <param name="_swapItem">��������</param>
    /// <param name="_curParent">��ǰ����</param>
    public void Swap(Item _swapItem, Transform _curParent)
    {
        _swapItem.transform.SetParent(rawParent);
        _swapItem.transform.localPosition = Vector3.zero;
        this.transform.SetParent(_curParent);
        this.transform.localPosition = Vector3.zero;
    }

    public void OnPointerEnter()
    {
        itemInfoPanel = GameObject.Instantiate<ItemInfoPanel>(itemInfoPrefab);
        itemInfoPanel.Init(ItemPro, gameObject.transform);
    }

    public void OnPointerExit()
    {
        if (itemInfoPanel)
        {
            GameObject.Destroy(itemInfoPanel.gameObject);
        }
    }

    public void OnDrag()
    {
        if (Input.GetMouseButton(0))
        {
            //�����������ɱ��ഴ������item��
            if (itemInfoPanel)
            {
                Destroy(itemInfoPanel.gameObject);
            }
            transform.position = Input.mousePosition;
        }
    }

    public void OnBeginningDrag()
    {
        if (Input.GetMouseButton(0))
        {
            //�����ƶ�ǰ�ĸ���
            rawParent = transform.parent;
            //��ǰ��
            transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            transform.localPosition = Vector3.zero;
        }
    }

    public void OnEndDrag()
    {
        var isInBagGrid = RectTransformUtility.RectangleContainsScreenPoint(BagPanel.Instance.transform.GetComponent<RectTransform>(), Input.mousePosition);
        //����ʱ������������
        if (BagPanel.Instance.gameObject.activeSelf)
        {
            //����ʱ����ڱ�������
            if (isInBagGrid)
            {
                //������������
                foreach (var grid in BagPanel.Instance.Grids)
                {
                    //����ڱ���������
                    if (grid.IsInGrid(Input.mousePosition))
                    {
                        //���������
                        if (grid.transform.childCount == 0)
                        {
                            transform.SetParent(grid.transform);
                            transform.localPosition = Vector3.zero;

                            //�Ƿ�װ��������
                            if (rawParent.GetComponent<Equip_Grid>() != null)
                            {
                                //�Ƴ�EquipList���޸�����
                                Character_Model.Instancce.DropEquip(ItemPro as Equip_Product);
                                //���뱳���б�
                                Bag_Model.Instance.bagItemsList.Add(ItemPro);
                            }
                            return;
                        }
                        else
                        {
                            //�Ƿ�װ��������
                            if (rawParent.GetComponent<Equip_Grid>() != null)
                            {
                                //��ǰ����
                                var dragEquip = ItemPro as Equip_Product;
                                //��������
                                var swapItem = grid.transform.GetChild(0).GetComponent<Item>();
                                var swapEquip = swapItem.ItemPro as Equip_Product;
                                //��������ת�����ͳɹ���˵��Ϊװ��
                                if (swapEquip != null)
                                {
                                    //װ��������ͬ
                                    if (dragEquip.Equip_Type == swapEquip.Equip_Type)
                                    {
                                        //��������
                                        Swap(swapItem, grid.transform);
                                        //�Ƴ������BagList
                                        Bag_Model.Instance.bagItemsList.Add(ItemPro);
                                        Bag_Model.Instance.bagItemsList.Remove(swapEquip);
                                        //�Ƴ������EquipList����������
                                        Character_Model.Instancce.DropEquip(dragEquip);
                                        Character_Model.Instancce.AddEquip(swapEquip);
                                        //˫�������Ͷ��ƻ������
                                        UpdateHands(swapEquip);
                                        return;
                                    }
                                }
                                //֪ͨ������װ��
                            }
                            else
                            {
                                var swapItem = grid.transform.GetChild(0).GetComponent<Item>();
                                Swap(swapItem, grid.transform);
                                return;
                            }
                        }
                    }
                }
            }
        }

        var isInEquipGrid = RectTransformUtility.RectangleContainsScreenPoint(CharacterPanel.Instance.transform.GetComponent<RectTransform>(), Input.mousePosition);
        //����ʱ������������
        if (CharacterPanel.Instance.gameObject.activeSelf)
        {
            //����ʱ�����װ������
            if (isInEquipGrid)
            {
                //�����������װ��
                var dragEquip = ItemPro as Equip_Product;
                if (dragEquip != null)
                {
                    //����װ��������
                    foreach (var grid in CharacterPanel.Instance.EquipGrids)
                    {
                        //�������������
                        if (grid.IsInEquipGrid(Input.mousePosition))
                        {
                            //װ��������ͬʱ
                            if (dragEquip.Equip_Type == grid.equipType)
                            {
                                //����װ��
                                if (grid.transform.childCount != 1)
                                {
                                    var swapItem = grid.transform.GetChild(1).GetComponent<Item>();
                                    var swapEquip = swapItem.ItemPro as Equip_Product;
                                    Swap(swapItem, grid.transform);
                                    //�޸ı����б�
                                    Bag_Model.Instance.AddListItem(swapEquip);
                                    Bag_Model.Instance.DropListItem(ItemPro);
                                    //�޸Ľ�ɫ����
                                    Character_Model.Instancce.DropEquip(swapEquip);
                                    Character_Model.Instancce.AddEquip(dragEquip);
                                    //��������ų�
                                    UpdateHands(dragEquip);
                                    return;
                                }
                                else
                                {
                                    transform.SetParent(grid.transform);
                                    transform.localPosition = Vector3.zero;
                                    //�޸ı����б�
                                    Bag_Model.Instance.DropListItem(ItemPro);
                                    //�޸Ľ�ɫ����
                                    Character_Model.Instancce.AddEquip(dragEquip);
                                    //��������ų�
                                    UpdateHands(dragEquip);
                                    return;
                                }
                            }
                        }
                    }
                    //֪ͨ����Ʒ���Ͳ���
                }
            }
        }
        transform.SetParent(rawParent);
        transform.localPosition = Vector3.zero;

        if (!isInBagGrid && !isInEquipGrid)
        {
            if (rawParent.GetComponent<Equip_Grid>())
            {
                SubmitPanel.Instance.inEquipGrid = true;
            }
            else
            {
                SubmitPanel.Instance.inEquipGrid = false;
            }
            SubmitPanel.Instance.titleTxt.text = "ȷ��Ҫ������Ʒ��";
            SubmitPanel.Instance.itemPro = ItemPro;
            SubmitPanel.Instance.infoTxt.text = ItemPro.ItemName;
            SubmitPanel.Instance.type = SubmitType.dropItem;
            SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
        }

        #region ʹ��SetActive�������ʱ�ж�
        //if (rawParent.GetComponent<Equip_Grid>() && !isInEquipGrid && BagPanel.Instance.transform.localScale == Vector3.zero)//&&BagPanel.Instance.gameObject.activeSelf==false
        //{
        //    SubmitPanel.Instance.inEquipGrid = true;
        //    SubmitPanel.Instance.titleTxt.text = "ȷ��Ҫ����װ����";
        //    SubmitPanel.Instance.itemPro = ItemPro;
        //    SubmitPanel.Instance.infoTxt.text = ItemPro.ItemName;
        //    SubmitPanel.Instance.type = SubmitType.dropItem;
        //    SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);

        //}
        //else if (rawParent.GetComponent<Bag_Grid>() && !isInBagGrid && CharacterPanel.Instance.transform.localScale == Vector3.zero)//&& CharacterPanel.Instance.GetComponent<CanvasGroup>().alpha == 0
        //{
        //    SubmitPanel.Instance.inEquipGrid = true;
        //    SubmitPanel.Instance.titleTxt.text = "ȷ��Ҫ������Ʒ��";
        //    SubmitPanel.Instance.itemPro = ItemPro;
        //    SubmitPanel.Instance.infoTxt.text = ItemPro.ItemName;
        //    SubmitPanel.Instance.type = SubmitType.dropItem;
        //    SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
        //}
    }
    #endregion

    public void OnMouseClick(BaseEventData baseED)
    {
        PointerEventData pointerED = baseED as PointerEventData;
        switch (pointerED.button)
        {
            //���ȡ���������
            case PointerEventData.InputButton.Left:
                if (itemInfoPanel) Destroy(itemInfoPanel.gameObject);
                break;
            case PointerEventData.InputButton.Right:
                switch (ItemPro.Item_Type)
                {
                    case Items_Product.ItemType.None:
                        break;
                    //ʹ��ҩƷ
                    case Items_Product.ItemType.IT_Drug:
                        Drug_Product drugPro = ItemPro as Drug_Product;
                        //��������
                        Character_Model.Instancce.UseDrug(drugPro);

                        //ɾ��ҩƷ
                        Bag_Model.Instance.DropItem(drugPro, true);
                        MessageEventSystem.Instance.SendEventMessage(EventMessage.hpUpdateMes);
                        //ȡ���������
                        if (itemInfoPanel) Destroy(itemInfoPanel.gameObject);
                        break;
                    case Items_Product.ItemType.IT_Clutter:
                        //����
                        //����
                        break;
                    case Items_Product.ItemType.IT_Equip:
                        //ʹ��װ��ǰ����ԭitem����
                        rawParent = transform.parent;
                        Equip_Product curEquip = ItemPro as Equip_Product;
                        //�ҵ���װ���������������
                        foreach (var grid in CharacterPanel.Instance.EquipGrids)
                        {
                            if (grid.equipType == curEquip.Equip_Type)
                            {
                                //����װ������
                                if (grid.transform.childCount != 1)
                                {
                                    //��������������
                                    var swapItem = grid.transform.GetChild(1).GetComponent<Item>();
                                    Swap(swapItem, grid.transform);
                                    var swapItemPro = swapItem.ItemPro as Equip_Product;
                                    Character_Model.Instancce.DropEquip(swapItemPro);
                                    Character_Model.Instancce.AddEquip(ItemPro as Equip_Product);
                                    Bag_Model.Instance.bagItemsList.Add(swapItemPro);
                                    Bag_Model.Instance.bagItemsList.Remove(ItemPro);
                                }
                                else
                                {
                                    //����������
                                    transform.SetParent(grid.transform);
                                    transform.localPosition = Vector3.zero;
                                    Character_Model.Instancce.AddEquip(curEquip);
                                    Bag_Model.Instance.bagItemsList.Remove(ItemPro);
                                }
                                //˫�������Ͷ��ƻ������
                                UpdateHands(curEquip);
                            }
                            if (itemInfoPanel) Destroy(itemInfoPanel.gameObject);
                        }
                        break;
                    default:
                        break;
                }
                break;
            case PointerEventData.InputButton.Middle:
                break;
            default:
                break;
        }
    }

    public void OtherBagItemChange()
    {
        if (transform.parent.name == "OtherConPanel")
        {

        }
    }

    //�ҵ���������
    public Item GetWeaponItem()
    {
        foreach (var grid in CharacterPanel.Instance.EquipGrids)
        {
            if (grid.equipType == Equip_Product.EquipType.ET_Weapons)
            {
                if (grid.transform.childCount != 1)
                {
                    var equipItem = grid.transform.GetChild(1).GetComponent<Item>();
                    return equipItem;
                }
            }
        }
        return null;
    }

    //�ҵ���������
    public Item GetShieldItem()
    {
        foreach (var grid in CharacterPanel.Instance.EquipGrids)
        {
            if (grid.equipType == Equip_Product.EquipType.ET_Shields)
            {
                if (grid.transform.childCount != 1)
                {
                    var equipItem = grid.transform.GetChild(1).GetComponent<Item>();
                    return equipItem;
                }
            }
        }
        return null;
    }

    public void UpdateHands(Equip_Product _equipPro)
    {
        //�����˫������װ����ж�¸���
        if (_equipPro.TypeName.Equals("˫��"))
        {
            var shieldItem = GetShieldItem();
            if (shieldItem != null)
            {
                //ж�¶���
                var nullBagGrid = BagPanel.Instance.FindNullGrid();
                shieldItem.transform.SetParent(nullBagGrid.transform);
                shieldItem.transform.localPosition = Vector3.zero;
                Bag_Model.Instance.AddListItem(shieldItem.ItemPro);
                Character_Model.Instancce.DropEquip(shieldItem.ItemPro as Equip_Product);
            }
        }
        else if (_equipPro.TypeName.Equals("����"))
        {
            var weaponItem = GetWeaponItem();
            if (weaponItem != null)
            {
                if (weaponItem.ItemPro.TypeName.Equals("˫��"))
                {
                    //ж��˫������
                    var nullBagGrid = BagPanel.Instance.FindNullGrid();
                    weaponItem.transform.SetParent(nullBagGrid.transform);
                    weaponItem.transform.localPosition = Vector3.zero;
                    Bag_Model.Instance.AddListItem(weaponItem.ItemPro);
                    Character_Model.Instancce.DropEquip(weaponItem.ItemPro as Equip_Product);
                }
            }
        }
    }
}

