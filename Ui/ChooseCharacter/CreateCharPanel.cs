using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
������ɫ���
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
        //Ĭ������
        showImage.sprite = Resources.Load<Sprite>("CharacterImage/warrior_silhouette_man");
        showTxt.text = "�����ߣ�         " +
            "�����Ŀ��ᣬ�Ŷӵ�ǰ�棬��η������ӵ��ǿ���ķ��������������������˺��ܵ͡�";
        type = (int)Character_Product.CharType.CT_Defender;
    }

    private void OnClickConfirmBtn()
    {
        //���ı���ѡ��ť������˸��
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
        //ȥ�����������ո�������ʽ\S�������������ַ��������������
        //string name = Regex.Replace(nameInput.text, @"\s", "");
        string name = nameInput.text;
        if (name == "")
        {
            errorTips.text = "�������������";
            return;
        }
        else
        {
            errorTips.text = "";
            if (CharList_Model.Instance.NameExists(name))
            {
                errorTips.text = "�����Ѵ��ڣ�����������";
                return;
            }
            else
            {
                //������ɫ
                CharList_Model.Instance.SaveCharacterInfo(name,type);
                //��ת
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
        showTxt.text = "���֣�          " +
            "ɱ����ǿ�󣬵����������ͷ�������ѷɫ��࣬��������޷���Ч��������Ҫ���˵ı���";
        type = (int)Character_Product.CharType.CT_Archer;
    }

    private void OnClickMeleeBtn()
    {
        showImage.sprite = Resources.Load<Sprite>("CharacterImage/warrior_silhouette_man");
        showTxt.text = "սʿ��          " +
            "�˺��ͷ���ƽ�⣬�ʺϽ��������޷�Զ�̹������֣������������Ѹ���������˺�";
        type = (int)Character_Product.CharType.CT_Melee;
    }

    private void OnClickDefenderBtn()
    {
        showImage.sprite = Resources.Load<Sprite>("CharacterImage/warrior_silhouette_man");
        showTxt.text = "�����ߣ�" +
            "�����Ŀ��ᣬ�Ŷӵ�ǰ�棬��η������ӵ��ǿ���ķ��������������������˺��ܵ͡�";
        type = (int)Character_Product.CharType.CT_Defender;
    }
}

