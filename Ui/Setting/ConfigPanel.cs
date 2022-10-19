using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
ESC����������������Ϸ���ã���Ϸ���ܣ��˳�������
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
        SubmitPanel.Instance.titleTxt.text = "ȷ��Ҫ�˳���Ϸ��";
        SubmitPanel.Instance.type = SubmitType.exitGame;
        SubmitPanel.Instance.transform.parent.gameObject.SetActive(true);
    }
    public void OnClickBackBtn()
    {
        gameObject.SetActive(false);
    }
}

