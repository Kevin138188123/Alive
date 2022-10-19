using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/*
Ê±ÖÓ¿ØÖÆÆ÷
*/
public class ClockPanel : MonoBehaviour
{
    TextMeshProUGUI timeTxt;

    private void Start()
    {
        timeTxt = transform.Find("TimeTxt").GetComponent<TextMeshProUGUI>();
    }

    
    private void FixedUpdate()
    {
        DateTime dateTime = DateTime.Now;
        timeTxt.text = dateTime.ToString("HH:mm:ss");
    }
}

