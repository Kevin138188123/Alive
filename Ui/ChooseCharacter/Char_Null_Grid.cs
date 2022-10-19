using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/*
��ɫѡ������еĴ�������
*/
public class Char_Null_Grid : MonoBehaviour
{
    public Transform choosePanel;
    public Transform createPanel;

    //��̬��������Ҫ��Awake�л�ȡ���
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
        //ע��Ҫ������ϵ������Ϊfalse����������������
        transform.SetParent(_parent,false);
        transform.localPosition = Vector3.zero;
    }

    private void OnClickCreateBtn()
    {
        choosePanel.gameObject.SetActive(false);
        createPanel.gameObject.SetActive(true);
    }
}

