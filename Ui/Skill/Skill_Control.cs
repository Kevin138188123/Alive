using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/*
单个技能控制器
*/
public class Skill_Control : MonoBehaviour, IPointerClickHandler//鼠标事件接口
{
    public float cdTime;
    public KeyCode key;
    bool isCD;
    Image fillCount;
    TextMeshProUGUI timeTxt;

    private void Awake()
    {
        timeTxt = transform.Find("Txt").GetComponent<TextMeshProUGUI>();
        fillCount = transform.Find("Fill").GetComponent<Image>();
        timeTxt.text = "";
        fillCount.fillAmount = 0f;
        isCD = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key) && isCD == false)
        {
            //动画处理，数据处理
            fillCount.fillAmount = 1;
            timeTxt.text = cdTime+"s";
            isCD = true;
            StartCoroutine("StartTiming");
        }
    }

    IEnumerator StartTiming()
    {
        float overTime = cdTime;
        while (true)
        {
            yield return new WaitForSeconds(0.1f);//*
            overTime -= 0.1f;
            fillCount.fillAmount = overTime / cdTime;
            timeTxt.text=overTime.ToString("0.0");
            if (overTime <= 0)
            {
                isCD = false;
                timeTxt.text = "";
                fillCount.fillAmount = 0f;
                yield break;
            }
        }
    }

    //鼠标点击事件函数（PointerEventData eventData）
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isCD == false)
        {
            //动画处理，数据处理
            fillCount.fillAmount = 1;
            timeTxt.text = cdTime.ToString();
            isCD = true;
            StartCoroutine(StartTiming());
        }
    }
}

