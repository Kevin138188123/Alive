using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
异步加载控制
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
        //判断进入游戏
        AppConst.isLogin = true;
    }

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        operation = SceneManager.LoadSceneAsync("MainScene");

        while (true)//偶尔卡死
        {
            fillImage.fillAmount = operation.progress;
            progressTxt.text = operation.progress * 100 + "%";
            yield return null;
        }
        //while (!operation.isDone)//偶尔卡死
        //{
        //    Debug.Log("我在读取中。。。");
        //    fillImage.fillAmount = operation.progress;
        //    progressTxt.text = operation.progress * 100 + "%";
        //    yield return null;
        //}
    }

}

