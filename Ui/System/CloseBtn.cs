using System.Collections.Generic;
using UnityEngine;
/*
关闭按钮
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
        //缩放隐藏不会检测到鼠标
        target.transform.localScale = Vector3.zero;
        //禁用隐藏仍然能检测鼠标
        //target.gameObject.SetActive(false);
    }
}

