using System.Collections.Generic;
using UnityEngine;
/*
ģ����ҹ
*/
public class SunMove : MonoBehaviour
{
    public float angle;
   
    private void Update()
    {
        transform.Rotate(Vector3.forward, angle * Time.deltaTime, Space.World);
    }
}

