using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
ESC控制面板控制器：游戏配置，游戏介绍，退出，返回
*/
public class ConfigPanel : MonoBehaviour
{
    public Transform settingPanel;
    public Transform gameInfoPanel;

    Button gameConfigBtn;
    Button gameInfoBtn;
    Button exitBtn;
    Button backBtn;

    private void Awake()
    {
        gameConfigBtn = transform.Find("GameConfigBtn").GetComponent<Button>();
        gameInfoBtn = transform.Find("GameInfoBtn").GetComponent<Button>();
        exitBtn = transform.Find("ExitBtn").GetComponent<Button>();
        backBtn = transform.Find("BackBtn").GetComponent<Button>();
        gameConfigBtn.onClick.AddListener(OnClickConfigBtn);
        gameInfoBtn.onClick.AddListener(OnClickGameInfoBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
        backBtn.onClick.AddListener(OnClickBackBtn);
    }

    public void OnClickConfigBtn()
    {
        settingPanel.gameObject.SetActive(true);
        settingPanel.transform.SetAsLastSibling();
    }
    public void OnClickGameInfoBtn()
    {
        gameInfoPanel.gameObject.SetActive(true);
        gameInfoPanel.transform.SetAsLastSibling();

    }
    public void OnClickExitBtn()
    {
        SubmitPanel.Instance.titleTxt.text = "确定要退出游戏吗？";
        SubmitPanel.Instance.type = SubmitType.exitGame;
        SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
    }
    public void OnClickBackBtn()
    {
        gameObject.SetActive(false);
    }
}

