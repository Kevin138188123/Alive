using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
ѡ���ɫ���
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
    #region ����
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

    //����ʱ���ؽ�ɫ�б����Grid
    private void OnEnable()
    {
        Init();
    }
    //����ʱ��ս�ɫ�б�����ظ���¼���ظ���ӵ�Bug
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
            //�ڿ���������Ӵ�����ɫ��ť
            foreach (var character in CharList_Model.Instance.characterList)
            {
                AddCharGrid(character);
            }
            //ָ�������չʾ�Ľ�ɫ

            if (charList.Count == 4) return;
        }
        AddNullGrid();
    }

    public void AddCharGrid(Character_Product _character)
    {
        //�ڿ���������ӽ�ɫ��Ϣ��ť
        var gridPrefab = Resources.Load<Char_Grid>("Prefabs/Char_Grid");
        var charGrid = GameObject.Instantiate(gridPrefab);
        //��Awake�л�ȡ�������Start�л�ȡΪnull
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
            //�ڿ���������ӽ�ɫ��Ϣ��ť
            var gridPrefab = Resources.Load<Char_Null_Grid>("Prefabs/Char_Null_Grid");
            nullGrid = GameObject.Instantiate(gridPrefab);
            //��Awake�л�ȡ�������Start�л�ȡΪnull
            nullGrid.Init(areaPanel, transform, createPanel);
        }
    }

    private void OnClickBackBtn()
    {
        //���ص�¼����ʱ���ѡ��Ľ�ɫId
        errorTxt.text = "";
        AppConst.charId = 0;
        loginPanel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnClickConfirmBtn()
    {

        if (AppConst.charId == 0)
        {
            errorTxt.text = "��ʾ������ѡ���ɫ��û�н�ɫ����������ť��";
        }
        else
        {
            SceneManager.LoadScene("AsyncLoading");
        }
    }
}

