using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
Text���������
*/

public enum TextType
{
    TT_Name,
    TT_Level,
    TT_Species,
    TT_Appellation,
    TT_Career,
    TT_Organization,
    TT_Hp,
    TT_Mp,
    TT_Atk,
    TT_Def,
    TT_Exp,
}

public class AttributesType : MonoBehaviour
{
    public TextType attributesType;

    //�ű����ԣ���ȡ��������
    public TextMeshProUGUI textComponent
    {
        get { return transform.GetComponent<TextMeshProUGUI>(); }
    }
}

