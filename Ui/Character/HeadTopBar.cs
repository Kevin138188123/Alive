
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
��ɫͷ��Ѫ��������ʾ
*/
public class HeadTopBar : MonoBehaviour
{
    TextMeshProUGUI nameTxt;
    Image hpFill;

    private void Awake()
    {
        nameTxt = transform.Find("NameTxt").GetComponent<TextMeshProUGUI>();
        hpFill = transform.Find("HpBar").Find("HpFill").GetComponent<Image>();
        UpdateHeadTop();
        MessageEventSystem.Instance.AddListener(EventMessage.hpUpdateMes,UpdateHeadTop);
    }

    private void UpdateHeadTop()
    {
        var charPro = Character_Model.Instancce.character;
        if (charPro == null)
        {
            Debug.LogError("δ���ص���ɫ��Ϣ");
            return;
        }
        nameTxt.text = charPro.Name;
        hpFill.fillAmount = charPro.Hp * 1f / charPro.MaxHp;
    }
}

