using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
创建角色面板
*/
public class CreateCharPanel : MonoBehaviour
{
    Button defenderBtn;
    Button meleeBtn;
    Button archerBtn;
    Button confirmBtn;
    Button backBtn;
    Image showImage;
    TextMeshProUGUI showTxt;
    TMP_InputField nameInput;
    TextMeshProUGUI errorTips;
    Character_Product characterPro;
    int type;

    public Transform choosePanel;

    private void Start()
    {
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        nameInput = transform.Find("NameInput").GetComponent<TMP_InputField>();
        errorTips = nameInput.transform.Find("ErrorTips").GetComponent<TextMeshProUGUI>();
        showImage = transform.Find("ShowPanel").Find("ShowImage").GetComponent<Image>();
        showTxt = transform.Find("ShowPanel").Find("ShowTxt").GetComponent<TextMeshProUGUI>();
        defenderBtn = transform.Find("ChooseGrids").Find("Defender").GetComponent<Button>();
        meleeBtn = transform.Find("ChooseGrids").Find("Melee").GetComponent<Button>();
        archerBtn = transform.Find("ChooseGrids").Find("Archer").GetComponent<Button>();
        defenderBtn.onClick.AddListener(OnClickDefenderBtn);
        meleeBtn.onClick.AddListener(OnClickMeleeBtn);
        archerBtn.onClick.AddListener(OnClickArcherBtn);
        backBtn.onClick.AddListener(OnClickBackBtn);
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        //默认设置
        showImage.sprite = Resources.Load<Sprite>("CharacterImage/warrior_silhouette_man");
        showTxt.text = "防御者：         " +
            "天生的抗揍，团队的前锋，无畏生死。拥有强悍的防御力和生命力，但是伤害很低。";
        type = (int)Character_Product.CharType.CT_Defender;
    }

    private void OnClickConfirmBtn()
    {
        //不改变已选按钮（会闪烁）
        EventSystem.current.SetSelectedGameObject(null);
        switch (type)
        {
            case 0:
                EventSystem.current.SetSelectedGameObject(defenderBtn.gameObject);
                break;
            case 1:
                EventSystem.current.SetSelectedGameObject(meleeBtn.gameObject);
                break;
            case 2:
                EventSystem.current.SetSelectedGameObject(archerBtn.gameObject);
                break;
            default:
                break;
        }
        //去除特殊符号与空格正则表达式\S代表所有特殊字符，面板中已限制
        //string name = Regex.Replace(nameInput.text, @"\s", "");
        string name = nameInput.text;
        if (name == "")
        {
            errorTips.text = "请输入玩家名字";
            return;
        }
        else
        {
            errorTips.text = "";
            if (CharList_Model.Instance.NameExists(name))
            {
                errorTips.text = "名字已存在，请重新输入";
                return;
            }
            else
            {
                //创建角色
                CharList_Model.Instance.SaveCharacterInfo(name,type);
                //跳转
                choosePanel.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnClickBackBtn()
    {
        nameInput.text = "";
        errorTips.text = "";
        choosePanel.gameObject.SetActive(true);
        transform.gameObject.SetActive(false);
    }

    private void OnClickArcherBtn()
    {
        showImage.sprite = Resources.Load<Sprite>("CharacterImage/warrior_silhouette_woman");
        showTxt.text = "射手：          " +
            "杀伤力强大，但是生命力和防御力会逊色许多，被近身后无法有效攻击，需要他人的保护";
        type = (int)Character_Product.CharType.CT_Archer;
    }

    private void OnClickMeleeBtn()
    {
        showImage.sprite = Resources.Load<Sprite>("CharacterImage/warrior_silhouette_man");
        showTxt.text = "战士：          " +
            "伤害和防御平衡，适合近身攻击，无法远程攻击对手，拉开距离后很难给对手造成伤害";
        type = (int)Character_Product.CharType.CT_Melee;
    }

    private void OnClickDefenderBtn()
    {
        showImage.sprite = Resources.Load<Sprite>("CharacterImage/warrior_silhouette_man");
        showTxt.text = "防御者：" +
            "天生的抗揍，团队的前锋，无畏生死。拥有强悍的防御力和生命力，但是伤害很低。";
        type = (int)Character_Product.CharType.CT_Defender;
    }
}

