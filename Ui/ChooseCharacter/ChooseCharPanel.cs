using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
选择角色面板
*/
public class ChooseCharPanel : MonoBehaviour
{
    Char_Grid[] grids;
    Button confirmBtn;
    Button backBtn;
    TextMeshProUGUI errorTxt;
    public Transform loginPanel;
    public Transform areaPanel;
    public Transform createPanel;
    Char_Null_Grid nullGrid = null;
    public Camera showCamera;
    #region 单例
    ChooseCharPanel() { }
    static ChooseCharPanel instance;
    static readonly object locker = new object();
    public static ChooseCharPanel Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new ChooseCharPanel();
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
        Character_Factory.Instance.LoadXml(AppConst.heroInfoXmlUrl);
        errorTxt = transform.Find("ErrorTxt").GetComponent<TextMeshProUGUI>();
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        backBtn.onClick.AddListener(OnClickBackBtn);
    }

    //启用时加载角色列表并添加Grid
    private void OnEnable()
    {
        Init();
    }
    //禁用时清空角色列表，解决重复登录会重复添加的Bug
    private void OnDisable()
    {
        for (int i = 0; i < areaPanel.childCount; i++)
        {
            GameObject.Destroy(areaPanel.GetChild(i).gameObject);
        }
    }

    public void Init()
    {
        var charList = CharList_Model.Instance.characterList;
        if (charList.Count != 0)
        {
            //var a=UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            //在空网格中添加创建角色按钮
            foreach (var character in CharList_Model.Instance.characterList)
            {
                AddCharGrid(character);
            }
            //指定摄像机展示的角色

            if (charList.Count == 4) return;
        }
        AddNullGrid();
    }

    public void AddCharGrid(Character_Product _character)
    {
        //在空网格中添加角色信息按钮
        var gridPrefab = Resources.Load<Char_Grid>("Prefabs/Char_Grid");
        var charGrid = GameObject.Instantiate(gridPrefab);
        //在Awake中获取子组件，Start中获取为null
        charGrid.Init(_character, areaPanel);
    }

    public void RemoveCharGrid(Character_Product _character)
    {
        var grid = FindItem(_character);
        GameObject.Destroy(grid.gameObject);
        CharList_Model.Instance.RemoveCharacterInfo(_character);
    }

    public Char_Grid FindItem(Character_Product _character)
    {
        grids = areaPanel.GetComponentsInChildren<Char_Grid>();
        foreach (var grid in grids)
        {
            if (grid.character.Id == _character.Id)
            {
                return grid;
            }
        }
        return null;
    }

    public void AddNullGrid()
    {
        if (areaPanel.childCount < 4 && !nullGrid)
        {
            //在空网格中添加角色信息按钮
            var gridPrefab = Resources.Load<Char_Null_Grid>("Prefabs/Char_Null_Grid");
            nullGrid = GameObject.Instantiate(gridPrefab);
            //在Awake中获取子组件，Start中获取为null
            nullGrid.Init(areaPanel, transform, createPanel);
        }
    }

    private void OnClickBackBtn()
    {
        //返回登录界面时清空选择的角色Id
        errorTxt.text = "";
        AppConst.charId = 0;
        loginPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnClickConfirmBtn()
    {

        if (AppConst.charId == 0)
        {
            errorTxt.text = "提示：请先选择角色，没有角色请点击创建按钮！";
        }
        else
        {
            SceneManager.LoadScene("AsyncLoading");
        }
    }
}

