using System.Collections.Generic;
using UnityEngine;
/*
1.待机状态执行3次动画，动画事件
3.人物状态机完善攻击部分
4.开始UGUI制作
*/
public class BaerAnimatorEvents : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


}

