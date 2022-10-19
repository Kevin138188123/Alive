using System.Collections.Generic;
using UnityEngine;
/*
С��ͼ������
*/
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        transform.position= target.position + offset;
    }
}

