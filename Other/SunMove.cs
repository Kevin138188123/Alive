using System.Collections.Generic;
using UnityEngine;
/*
Ä£ÄâÖçÒ¹
*/
public class SunMove : MonoBehaviour
{
    public float angle;
   
    private void Update()
    {
        transform.Rotate(Vector3.forward, angle * Time.deltaTime, Space.World);
    }
}

