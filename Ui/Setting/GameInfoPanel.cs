using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
游戏信息面板控制器：游戏背景，玩法介绍
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