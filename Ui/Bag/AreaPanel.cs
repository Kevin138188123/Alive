using System.Collections.Generic;
using UnityEngine;
/*
区域面板，判断物品栏鼠标进入区域
*/
public class AreaPanel : MonoBehaviour
{
    //多个属性相同面板直接传入数组
    public RectTransform[] areaPanels;

    public Vector2 GetPointInArea(Vector3 _mousePosition)
    {
        foreach (var panel in areaPanels)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(panel, _mousePosition))
            {
                switch (panel.name)
                {
                    //子物体的中心点
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

