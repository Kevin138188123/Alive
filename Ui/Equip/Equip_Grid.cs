using System.Collections.Generic;
using UnityEngine;
/*
装备网格控制器
*/
public class Equip_Grid : MonoBehaviour
{
    //网格类型辨别类型
    public Equip_Product.EquipType equipType;

    public bool IsInEquipGrid(Vector3 _position)
    {
        var rect = transform.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rect, _position);

    }
}

