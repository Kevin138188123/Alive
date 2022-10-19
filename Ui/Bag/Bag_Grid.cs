using System.Collections.Generic;
using UnityEngine;
/*
±³°üÍø¸ñ¿ØÖÆÆ÷
*/
public class Bag_Grid : MonoBehaviour
{
    public bool IsInGrid(Vector3 _position)
    {
        var rect=transform.GetComponent<RectTransform>();
        return RectTransformUtility.RectangleContainsScreenPoint(rect,_position);
    }
}

