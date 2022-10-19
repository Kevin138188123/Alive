using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
角色选择面板中的角色网格
*/
public class Char_Grid : MonoBehaviour
{
    //存有角色信息，鼠标双击时可以直接拿到对象(点击按钮将信息存入常量类变量中)
    public Character_Product character;

    TextMeshProUGUI nameTxt;
    TextMeshProUGUI levelTxt;
    TextMeshProUGUI careerTxt;
    TextMeshProUGUI speciesTxt;
    Button removeBtn;

    #region 单例
    //CharGrid() { }
    //static CharGrid instance;
    //static readonly object locker = new object();
    //public static CharGrid Instance
    //{
    //    get 
    //    {
    //        if (instance == null)
    //        {
    //            lock (locker)
    //            {
    //                if (instance == null)
    //                {
    //                    instance = new CharGrid();
    //                }
    //            }
    //        }
    //        return instance;
    //    } 
    //}
    #endregion

    //动态添加组件需要在Awake中获取组件
    private void Awake()
    {
        //instance = this;
        nameTxt = transform.Find("NameTxt").GetComponent<TextMeshProUGUI>();
        levelTxt = transform.Find("LevelTxt").GetComponent<TextMeshProUGUI>();
        careerTxt = transform.Find("CareerTxt").GetComponent<TextMeshProUGUI>();
        speciesTxt = transform.Find("SpeciesTxt").GetComponent<TextMeshProUGUI>();
        //transform.GetComponent<Button>().onClick.AddListener(OnClickCharGrid);
        removeBtn= transform.Find("RemoveBtn").GetComponent<Button>();
        removeBtn.onClick.AddListener(OnClickRemove);
        transform.GetComponent<Button>().onClick.AddListener(OnClickEnterBtn);
    }

    private void OnClickEnterBtn()
    {
        AppConst.charId = character.Id;
    }

    public void Init(Character_Product _character, Transform _parent)
    {
        character = _character;
        nameTxt.text = _character.Name;
        levelTxt.text = _character.Level.ToString();
        careerTxt.text = _character.Career;
        speciesTxt.text = _character.Species;
        //注意要把坐标系参数改为false
        transform.SetParent(_parent, false);
        transform.localPosition = Vector3.zero;
    }

    public void OnClickRemove()
    {
        SubmitPanel.Instance.titleTxt.text = "确定要删除角色吗？";
        SubmitPanel.Instance.charPro = character;
        SubmitPanel.Instance.infoTxt.text = character.Name;
        SubmitPanel.Instance.type = SubmitType.removeChar;
        SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
    }
   
}

