using System.Collections.Generic;
using UnityEngine;
/*
��һ�˳��ӽǣ����������
*/
public class FirstView_Ctrl : MonoBehaviour
{
    Transform target;
    public float mouseSpeed;
    float mouseX, mouseY;
    float offset;

    void Start()
    {
        if (!target)
        {
            Debug.LogError("Please assign 'Camera Target' in the inspector, fps controller will not work.");
            enabled = false;
            return;
        }
    }

    private void LateUpdate()
    {
        
    }



}

