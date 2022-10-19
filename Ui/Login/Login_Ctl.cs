using System.Collections.Generic;
using UnityEngine;
/*
��¼�������������¼��ע�ᣬ���õ�������ʾ������
*/
public class Login_Ctl : MonoBehaviour
{
    public Transform configPanel;
    public Transform settingPanel;
    public Transform gameInfoPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingPanel.gameObject.activeInHierarchy)
            {
                settingPanel.gameObject.SetActive(false);
                return;
            }
            else if (gameInfoPanel.gameObject.activeInHierarchy)
            {
                gameInfoPanel.gameObject.SetActive(false);
                return;
            }
            else
            {
                if (configPanel.gameObject.activeInHierarchy)
                {
                    configPanel.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    configPanel.gameObject.SetActive(true);
                    configPanel.SetAsLastSibling();
                    return;
                }
            }

        }

    }
}

