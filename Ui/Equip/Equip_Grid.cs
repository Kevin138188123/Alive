using System.Collections.Generic;
using UnityEngine;
/*
װ�����������
*/
public class Equip_Grid : MonoBehaviour
{
    //�������ͱ������
    public Equip_Product.EquipType equipType;

    public bool IsInEquipGrid(Vector3 _position)
    {
        var rect = transform.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rect, _position);

    }
}

