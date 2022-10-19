
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
角色头顶血条姓名显示
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
            Debug.LogError("未加载到角色信息");
            return;
        }
        nameTxt.text = charPro.Name;
        hpFill.fillAmount = charPro.Hp * 1f / charPro.MaxHp;
    }
}

