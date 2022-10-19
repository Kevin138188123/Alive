using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
确认面板显示、处理
*/
public class SubmitPanel : MonoBehaviour
{
    Button yesBtn;
    Button noBtn;

    //拖拽组件赋值，忘记或者修改脚本容易造成空引用，
    [NonSerialized]
    public TextMeshProUGUI titleTxt;
    [NonSerialized]
    public TextMeshProUGUI infoTxt;

    //执行对象
    public Character_Product charPro;
    public Items_Product itemPro;

    //隐藏需要脚本赋值的公共属性
    [NonSerialized]
    public string type;
    [NonSerialized]
    public bool isYes;
    [NonSerialized]
    public bool inEquipGrid;

    #region 单例
    SubmitPanel() { }
    static SubmitPanel instance;
    static readonly object locker = new object();
    public static SubmitPanel Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new SubmitPanel();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    private void Start()
    {
        instance = this;
        titleTxt = transform.Find("TitleTxt").GetComponent<TextMeshProUGUI>();
        infoTxt = transform.Find("InfoTxt").GetComponent<TextMeshProUGUI>();

        yesBtn = transform.Find("YesBtn").GetComponent<Button>();
        yesBtn.onClick.AddListener(IsYes);
        noBtn = transform.Find("NoBtn").GetComponent<Button>();
        noBtn.onClick.AddListener(IsNo);
        transform.parent.gameObject.SetActive(false);
    }

    void IsYes()
    {
        isYes = true;
        transform.parent.gameObject.SetActive(false);
    }

    void IsNo()
    {
        isYes = false;
        transform.parent.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        transform.parent.SetAsLastSibling();
    }

    private void OnDisable()
    {
        if (isYes)
        {
            switch (type)
            {
                case SubmitType.exitGame:
                    Debug.Log("模拟退出游戏");
                    //异步加载时改变为进入游戏，判断退出时是否保存背包和装备
                    if (AppConst.isLogin)
                    {
                        Bag_Model.Instance.SaveBagItem();
                        Equip_Model.Instance.SaveEquipItem();
                    }
                    Application.Quit();
                    break;
                case SubmitType.dropItem:
                    if (inEquipGrid)
                    {
                        Equip_Model.Instance.DropEquip(itemPro);
                    }
                    else
                    {
                        Bag_Model.Instance.DropItem(itemPro);
                    }
                    break;
                case SubmitType.removeChar:
                    ChooseCharPanel.Instance.RemoveCharGrid(charPro);
                    ChooseCharPanel.Instance.AddNullGrid();
                    break;
                case SubmitType.death:
                    SceneManager.LoadScene("DeathScene");
                    break;

            }
        }
    }
}

