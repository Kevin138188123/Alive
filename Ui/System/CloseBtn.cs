using System.Collections.Generic;
using UnityEngine;
/*
�رհ�ť
*/
public class CloseBtn : MonoBehaviour
{
    Transform target;

    private void Awake()
    {
        target = transform.parent; 
    }

    public void OnClickBtn()
    {
        //�������ز����⵽���
        target.transform.localScale = Vector3.zero;
        //����������Ȼ�ܼ�����
        //target.gameObject.SetActive(false);
    }
}

