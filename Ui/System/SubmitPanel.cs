using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
ȷ�������ʾ������
*/
public class SubmitPanel : MonoBehaviour
{
    Button yesBtn;
    Button noBtn;

    //��ק�����ֵ�����ǻ����޸Ľű�������ɿ����ã�
    [NonSerialized]
    public TextMeshProUGUI titleTxt;
    [NonSerialized]
    public TextMeshProUGUI infoTxt;

    //ִ�ж���
    public Character_Product charPro;
    public Items_Product itemPro;

    //������Ҫ�ű���ֵ�Ĺ�������
    [NonSerialized]
    public string type;
    [NonSerialized]
    public bool isYes;
    [NonSerialized]
    public bool inEquipGrid;

    #region ����
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
                    Debug.Log("ģ���˳���Ϸ");
                    //�첽����ʱ�ı�Ϊ������Ϸ���ж��˳�ʱ�Ƿ񱣴汳����װ��
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

