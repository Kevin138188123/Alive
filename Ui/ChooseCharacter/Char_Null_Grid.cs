using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
角色选择面板中的创建网格
*/
public class Char_Null_Grid : MonoBehaviour
{
    public Transform choosePanel;
    public Transform createPanel;

    //动态添加组件需要在Awake中获取组件
    private void Awake()
    {
        transform.GetComponent<Button>().onClick.AddListener(OnClickCreateBtn);
    }
    private void Start()
    {

    }

    public void Init(Transform _parent,Transform _choosePanel,Transform _createPanel)
    {
        choosePanel = _choosePanel;
        createPanel = _createPanel;
        //注意要把坐标系参数改为false，不适用世界坐标
        transform.SetParent(_parent,false);
        transform.localPosition = Vector3.zero;
    }

    private void OnClickCreateBtn()
    {
        choosePanel.gameObject.SetActive(false);
        createPanel.gameObject.SetActive(true);
    }
}

