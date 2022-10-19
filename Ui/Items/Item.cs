using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
物品项控制器
*/
public class Item : MonoBehaviour
{
    Items_Product itemPro;//用于初始化Item时接收对象，其他函数需要用到
    public Items_Product ItemPro
    {
        get { return itemPro; }
    }

    Image image;
    Sprite sprite;
    TextMeshProUGUI numTxt;
    ItemInfoPanel itemInfoPrefab;//Transform，Gameobject类型不合适时，使用类名对象
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

    //当前对象是Model层List中的对象，同一个内存地址，在MOdel层修改后直接修改了内存中的数据
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
    /// 物品互换
    /// </summary>
    /// <param name="_swapItem">交换对象</param>
    /// <param name="_curParent">当前网格</param>
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
            //详情面板对象由本类创建就在item内
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
            //保存移动前的父亲
            rawParent = transform.parent;
            //最前层
            transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
            transform.localPosition = Vector3.zero;
        }
    }

    public void OnEndDrag()
    {
        var isInBagGrid = RectTransformUtility.RectangleContainsScreenPoint(BagPanel.Instance.transform.GetComponent<RectTransform>(), Input.mousePosition);
        //结束时背包栏开启中
        if (BagPanel.Instance.gameObject.activeSelf)
        {
            //结束时鼠标在背包栏内
            if (isInBagGrid)
            {
                //遍历背包网格
                foreach (var grid in BagPanel.Instance.Grids)
                {
                    //鼠标在背包网格中
                    if (grid.IsInGrid(Input.mousePosition))
                    {
                        //进入空网格
                        if (grid.transform.childCount == 0)
                        {
                            transform.SetParent(grid.transform);
                            transform.localPosition = Vector3.zero;

                            //是否装备栏进入
                            if (rawParent.GetComponent<Equip_Grid>() != null)
                            {
                                //移除EquipList并修改属性
                                Character_Model.Instancce.DropEquip(ItemPro as Equip_Product);
                                //加入背包列表
                                Bag_Model.Instance.bagItemsList.Add(ItemPro);
                            }
                            return;
                        }
                        else
                        {
                            //是否装备栏进入
                            if (rawParent.GetComponent<Equip_Grid>() != null)
                            {
                                //当前对象
                                var dragEquip = ItemPro as Equip_Product;
                                //交换对象
                                var swapItem = grid.transform.GetChild(0).GetComponent<Item>();
                                var swapEquip = swapItem.ItemPro as Equip_Product;
                                //交换对象转换类型成功，说明为装备
                                if (swapEquip != null)
                                {
                                    //装备类型相同
                                    if (dragEquip.Equip_Type == swapEquip.Equip_Type)
                                    {
                                        //交换父亲
                                        Swap(swapItem, grid.transform);
                                        //移除、添加BagList
                                        Bag_Model.Instance.bagItemsList.Add(ItemPro);
                                        Bag_Model.Instance.bagItemsList.Remove(swapEquip);
                                        //移除、添加EquipList和增减属性
                                        Character_Model.Instancce.DropEquip(dragEquip);
                                        Character_Model.Instancce.AddEquip(swapEquip);
                                        //双手武器和盾牌互斥更新
                                        UpdateHands(swapEquip);
                                        return;
                                    }
                                }
                                //通知：不能装备
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
        //结束时背包栏开启中
        if (CharacterPanel.Instance.gameObject.activeSelf)
        {
            //结束时鼠标在装备栏中
            if (isInEquipGrid)
            {
                //如果本对象是装备
                var dragEquip = ItemPro as Equip_Product;
                if (dragEquip != null)
                {
                    //遍历装备栏网格
                    foreach (var grid in CharacterPanel.Instance.EquipGrids)
                    {
                        //如果鼠标进入网格
                        if (grid.IsInEquipGrid(Input.mousePosition))
                        {
                            //装备类型相同时
                            if (dragEquip.Equip_Type == grid.equipType)
                            {
                                //存在装备
                                if (grid.transform.childCount != 1)
                                {
                                    var swapItem = grid.transform.GetChild(1).GetComponent<Item>();
                                    var swapEquip = swapItem.ItemPro as Equip_Product;
                                    Swap(swapItem, grid.transform);
                                    //修改背包列表
                                    Bag_Model.Instance.AddListItem(swapEquip);
                                    Bag_Model.Instance.DropListItem(ItemPro);
                                    //修改角色属性
                                    Character_Model.Instancce.DropEquip(swapEquip);
                                    Character_Model.Instancce.AddEquip(dragEquip);
                                    //检测武器排斥
                                    UpdateHands(dragEquip);
                                    return;
                                }
                                else
                                {
                                    transform.SetParent(grid.transform);
                                    transform.localPosition = Vector3.zero;
                                    //修改背包列表
                                    Bag_Model.Instance.DropListItem(ItemPro);
                                    //修改角色属性
                                    Character_Model.Instancce.AddEquip(dragEquip);
                                    //检测武器排斥
                                    UpdateHands(dragEquip);
                                    return;
                                }
                            }
                        }
                    }
                    //通知：物品类型不符
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
            SubmitPanel.Instance.titleTxt.text = "确定要丢弃物品吗？";
            SubmitPanel.Instance.itemPro = ItemPro;
            SubmitPanel.Instance.infoTxt.text = ItemPro.ItemName;
            SubmitPanel.Instance.type = SubmitType.dropItem;
            SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
        }

        #region 使用SetActive隐藏组件时判断
        //if (rawParent.GetComponent<Equip_Grid>() && !isInEquipGrid && BagPanel.Instance.transform.localScale == Vector3.zero)//&&BagPanel.Instance.gameObject.activeSelf==false
        //{
        //    SubmitPanel.Instance.inEquipGrid = true;
        //    SubmitPanel.Instance.titleTxt.text = "确定要丢弃装备吗？";
        //    SubmitPanel.Instance.itemPro = ItemPro;
        //    SubmitPanel.Instance.infoTxt.text = ItemPro.ItemName;
        //    SubmitPanel.Instance.type = SubmitType.dropItem;
        //    SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);

        //}
        //else if (rawParent.GetComponent<Bag_Grid>() && !isInBagGrid && CharacterPanel.Instance.transform.localScale == Vector3.zero)//&& CharacterPanel.Instance.GetComponent<CanvasGroup>().alpha == 0
        //{
        //    SubmitPanel.Instance.inEquipGrid = true;
        //    SubmitPanel.Instance.titleTxt.text = "确定要丢弃物品吗？";
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
            //左键取消详情面板
            case PointerEventData.InputButton.Left:
                if (itemInfoPanel) Destroy(itemInfoPanel.gameObject);
                break;
            case PointerEventData.InputButton.Right:
                switch (ItemPro.Item_Type)
                {
                    case Items_Product.ItemType.None:
                        break;
                    //使用药品
                    case Items_Product.ItemType.IT_Drug:
                        Drug_Product drugPro = ItemPro as Drug_Product;
                        //增加属性
                        Character_Model.Instancce.UseDrug(drugPro);

                        //删除药品
                        Bag_Model.Instance.DropItem(drugPro, true);
                        MessageEventSystem.Instance.SendEventMessage(EventMessage.hpUpdateMes);
                        //取消详情面板
                        if (itemInfoPanel) Destroy(itemInfoPanel.gameObject);
                        break;
                    case Items_Product.ItemType.IT_Clutter:
                        //制造
                        //卖出
                        break;
                    case Items_Product.ItemType.IT_Equip:
                        //使用装备前保存原item父亲
                        rawParent = transform.parent;
                        Equip_Product curEquip = ItemPro as Equip_Product;
                        //找到与装备类型相符的网格
                        foreach (var grid in CharacterPanel.Instance.EquipGrids)
                        {
                            if (grid.equipType == curEquip.Equip_Type)
                            {
                                //存在装备交换
                                if (grid.transform.childCount != 1)
                                {
                                    //交换并更新属性
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
                                    //穿戴并更新
                                    transform.SetParent(grid.transform);
                                    transform.localPosition = Vector3.zero;
                                    Character_Model.Instancce.AddEquip(curEquip);
                                    Bag_Model.Instance.bagItemsList.Remove(ItemPro);
                                }
                                //双手武器和盾牌互斥更新
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

    //找到武器网格
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

    //找到盾牌网格
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
        //如果是双手武器装备后卸下副手
        if (_equipPro.TypeName.Equals("双手"))
        {
            var shieldItem = GetShieldItem();
            if (shieldItem != null)
            {
                //卸下盾牌
                var nullBagGrid = BagPanel.Instance.FindNullGrid();
                shieldItem.transform.SetParent(nullBagGrid.transform);
                shieldItem.transform.localPosition = Vector3.zero;
                Bag_Model.Instance.AddListItem(shieldItem.ItemPro);
                Character_Model.Instancce.DropEquip(shieldItem.ItemPro as Equip_Product);
            }
        }
        else if (_equipPro.TypeName.Equals("盾牌"))
        {
            var weaponItem = GetWeaponItem();
            if (weaponItem != null)
            {
                if (weaponItem.ItemPro.TypeName.Equals("双手"))
                {
                    //卸下双手武器
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

