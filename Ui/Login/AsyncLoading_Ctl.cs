using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
�첽���ؿ���
*/
public class AsyncLoading_Ctl : MonoBehaviour
{
    Image fillImage;
    TextMeshProUGUI progressTxt;
    AsyncOperation operation;

    private void Start()
    {
        fillImage = transform.Find("FillImage").GetComponent<Image>() ;
        progressTxt = transform.Find("ProgressTxt").GetComponent<TextMeshProUGUI>();
        StartCoroutine("LoadScene");
        //�жϽ�����Ϸ
        AppConst.isLogin = true;
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        operation = SceneManager.LoadSceneAsync("MainScene");

        while (true)//ż������
        {
            fillImage.fillAmount = operation.progress;
            progressTxt.text = operation.progress * 100 + "%";
            yield return null;
        }
        //while (!operation.isDone)//ż������
        //{
        //    Debug.Log("���ڶ�ȡ�С�����");
        //    fillImage.fillAmount = operation.progress;
        //    progressTxt.text = operation.progress * 100 + "%";
        //    yield return null;
        //}
    }

}

