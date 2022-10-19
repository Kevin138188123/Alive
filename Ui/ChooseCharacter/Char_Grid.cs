using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
��ɫѡ������еĽ�ɫ����
*/
public class Char_Grid : MonoBehaviour
{
    //���н�ɫ��Ϣ�����˫��ʱ����ֱ���õ�����(�����ť����Ϣ���볣���������)
    public Character_Product character;

    TextMeshProUGUI nameTxt;
    TextMeshProUGUI levelTxt;
    TextMeshProUGUI careerTxt;
    TextMeshProUGUI speciesTxt;
    Button removeBtn;

    #region ����
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

    //��̬��������Ҫ��Awake�л�ȡ���
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
        //ע��Ҫ������ϵ������Ϊfalse
        transform.SetParent(_parent, false);
        transform.localPosition = Vector3.zero;
    }

    public void OnClickRemove()
    {
        SubmitPanel.Instance.titleTxt.text = "ȷ��Ҫɾ����ɫ��";
        SubmitPanel.Instance.charPro = character;
        SubmitPanel.Instance.infoTxt.text = character.Name;
        SubmitPanel.Instance.type = SubmitType.removeChar;
        SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
    }
   
}

