using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
/*
面板拖动脚本 
*/
public class DragPanel : MonoBehaviour
{
    Vector3 offset;
    float minX,maxX,minY,maxY;
    RectTransform rt;

    public void OnDrag(PointerEventData eventData)
    {
        rt = transform.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out Vector3 globalMousePos))
        {
            SetDragRange(rt);
            rt.position = DragRangeLimit(globalMousePos + offset);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        rt = transform.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, null, out Vector3 globalMousePos))
        {
            offset = rt.position - globalMousePos;
        }
    }

    public void SetDragRange(RectTransform rt)
    {
        minX = rt.rect.width / 2;
        maxX = Screen.width - minX;
        minY = rt.rect.height / 2;
        maxY = Screen.height - minY;
    }

    Vector3 DragRangeLimit(Vector3 pos)
    {
        pos.x = Mathf.Clamp(pos.x,minX,maxX);
        pos.y = Mathf.Clamp(pos.y,minY,maxY);
        return pos;
    }

    public void DragRangeLimit(Transform tra)
    {
        var pos = tra.GetComponent<RectTransform>();
        float x = Mathf.Clamp(pos.position.x, pos.rect.width * 0.5f, Screen.width - (pos.rect.width * 0.5f));
        float y = Mathf.Clamp(pos.position.y, pos.rect.height * 0.5f, Screen.height - (pos.rect.height * 0.5f));
        pos.position = new Vector2(x, y);
    }    


    public void RestrictMoveInWindow(RectTransform rect)
    {
        Vector3 pos = rect.localPosition;

        pos.x = Mathf.Clamp(rect.localPosition.x, rect.rect.xMin, rect.rect.xMax);
        pos.y = Mathf.Clamp(rect.localPosition.y, rect.rect.yMin, rect.rect.yMax);

        rect.localPosition = pos;
    }



    public static bool SetUIArea(RectTransform target, Rect area, Transform canvas)
    {
        Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(canvas, target);

        if (null == area)
        {
            return false;
        }

        Vector2 delta = default(Vector2);
        if (bounds.center.x - bounds.extents.x < area.x)//target超出area的左边框
        {
            delta.x += Mathf.Abs(bounds.center.x - bounds.extents.x - area.x);
        }
        else if (bounds.center.x + bounds.extents.x > area.width / 2)//target超出area的右边框
        {
            delta.x -= Mathf.Abs(bounds.center.x + bounds.extents.x - area.width / 2);
        }

        if (bounds.center.y - bounds.extents.y < area.y)//target超出area上边框
        {
            delta.y += Mathf.Abs(bounds.center.y - bounds.extents.y - area.y);
        }
        else if (bounds.center.y + bounds.extents.y > area.height / 2)//target超出area的下边框
        {
            delta.y -= Mathf.Abs(bounds.center.y + bounds.extents.y - area.height / 2);
        }

        //加上偏移位置算出在屏幕内的坐标
        target.anchoredPosition += delta;

        return delta != default(Vector2);
    }

    
}

