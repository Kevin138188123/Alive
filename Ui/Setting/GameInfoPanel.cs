using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
��Ϸ��Ϣ������������Ϸ�������淨����
*/
public class GameInfoPanel : MonoBehaviour
{
    Button backBtn;

    private void Awake()
    {
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        backBtn.onClick.AddListener(OnClickBackBtn);
    }

    private void OnClickBackBtn()
    {
        gameObject.SetActive(false);
    }
}