using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
背包面板
*/
public class BagPanel : MonoBehaviour,IDragHandler,IBeginDragHandler
{
    //static会隐藏面板属性
    //详情面板需要区域面板的脚本函数，需要传入脚本对象
    public AreaPanel areaPanel;
    //grid的父亲对象，传入transform对象
    Transform contentPanel;
    Bag_Grid[] grids;
    RectTransform rt;

    public Bag_Grid[] Grids
    {
        get
        {
            if (grids == null)
            {
                grids = transform.Find("ContentPanel").GetComponentsInChildren<Bag_Grid>();
            }
            return grids;
        }
    }

    #region 单例
    BagPanel() { }
    static BagPanel instance;
    static readonly object locker = new object();
    public static BagPanel Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new BagPanel();
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
        contentPanel = transform.Find("ContentPanel").GetComponent<Transform>();
        //主界面加载时读取背包数据库信息
        Bag_Model.Instance.LoadBagInfo();
        grids = transform.Find("ContentPanel").GetComponentsInChildren<Bag_Grid>();
    }

    public void CreateGridItem(Items_Product _itemPro = null)//读取Model层的字典
    {
        var grid = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bag_Grid"));
        grid.transform.SetParent(contentPanel);
        if (_itemPro != null)
        {
            var item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
            item.transform.GetComponent<Item>().Init(_itemPro, grid.transform);
        }
    }

    public void UpdateBagPanel(Items_Product _itemPro, bool isDrop)
    {
        if (isDrop)
        {
            var item = FindItem(_itemPro);
            if (item != null)
            {
                DestroyImmediate(item.gameObject);
                return;
            }
            Debug.Log("没有匹配物品");
            return;
        }
        Bag_Grid grid = FindNullGrid();
        if (grid != null)
        {
            var item = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
            item.transform.GetComponent<Item>().Init(_itemPro, grid.transform);
        }
        else
        {
            Debug.Log("背包空间不足");
        }
    }

    public Bag_Grid FindNullGrid()
    {
        for (int i = 0; i < Grids.Length; i++)
        {
            if (Grids[i].transform.childCount == 0)
            {
                return Grids[i];
            }
        }
        return null;
    }

    internal void UpdateItemCount(Items_Product _itemPro)
    {
        var item = FindItem(_itemPro);
        if (item != null)
        {
            item.UpdateItemCount();
        }
    }

    public Item FindItem(Items_Product _itemPro)
    {
        for (int i = 0; i < Grids.Length; i++)
        {
            if (Grids[i].transform.childCount == 1)
            {
                var item = Grids[i].transform.GetChild(0).GetComponent<Item>();
                if (item.ItemPro == _itemPro)
                {
                    return item;
                }
            }
        }
        return null;
    }

    #region 鼠标拖拽
    Vector3 offset;
    float minX, maxX, minY, maxY;

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out Vector3 globalMousePos))
        {
            SetDragRange(rt);
            rt.position = DragRangeLimit(globalMousePos + offset);
        }
    }

    //ScreenPointToWorldPointInRectangle在rt中点击后返回true
    //将鼠标点击时的屏幕坐标转换为世界坐标，矩形-世界坐标=offset
    //1矩形，2事件发生时的坐标，3画布摄像机(可null)，4输出世界坐标参数
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out Vector3 globalMousePos))
        {
            offset = rt.position - globalMousePos;
        }
    }

    //坐标限制
    public void SetDragRange(RectTransform rt)
    {
        //不受pivot限制
        minX = rt.rect.width * rt.pivot.x;
        maxX = Screen.width - rt.rect.width * (1 - rt.pivot.x);
        minY = rt.rect.height * rt.pivot.y;
        maxY = Screen.height - rt.rect.height * (1 - rt.pivot.y);
        //minX = rt.rect.width / 2;
        //maxX = Screen.width - minX;
        //minY = rt.rect.height / 2;
        //maxY = Screen.height - minY;
    }

    Vector3 DragRangeLimit(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
    #endregion

}

