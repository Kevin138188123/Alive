using System.Collections.Generic;
using UnityEngine;
/*
������壬�ж���Ʒ������������
*/
public class AreaPanel : MonoBehaviour
{
    //���������ͬ���ֱ�Ӵ�������
    public RectTransform[] areaPanels;

    public Vector2 GetPointInArea(Vector3 _mousePosition)
    {
        foreach (var panel in areaPanels)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(panel, _mousePosition))
            {
                switch (panel.name)
                {
                    //����������ĵ�
                    case "TopLeft":
                        return new Vector2(0,1);
                    case "TopRight":
                        return new Vector2(1, 1);
                    case "BottomLeft":
                        return new Vector2(0, 0);
                    case "BottomRight":
                        return new Vector2(1, 0);
                    default:
                        break;
                }
            }
        }
        return new Vector2();
    }
}

