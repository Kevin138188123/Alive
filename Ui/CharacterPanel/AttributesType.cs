using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
Text组件控制器
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

    //脚本属性，获取对象的组件
    public TextMeshProUGUI textComponent
    {
        get { return transform.GetComponent<TextMeshProUGUI>(); }
    }
}

