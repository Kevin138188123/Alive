using System.Collections.Generic;
using UnityEngine;
/*
角色动画状态机
*/
public enum CharacterStateType
{
    Idle,
    WalkForward,
    WalkBackward,
    StrafeLeft,
    StrafeRight,
    TurnLeft,
    TurnRight,
    RunForward,
    RunBackward,
    Jump,
    RunJump,
    StunnedLoop,
    Death,
    Buff,
    //交流
    Hello,
    Talk,
    Listen,
    //生活技能
    Gather,
    Mining,
    //战斗
    C_Idle,
    C_PunchLeft,
    C_PunchRight,
    C_BlockingLoop,
    C_Hurt,
}

public class Character_Animator : MonoBehaviour
{
    Animator animator;

    CharacterStateType curState;
    CharacterStateType nextState;

    float horizontal;
    float vertical;
    bool isDeath;
    bool isRun;
    bool isHurt;
    bool isCombat;




    private void Start()
    {
        curState = CharacterStateType.Idle;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))//模拟死亡
        {
            nextState = CharacterStateType.Death;
            isDeath = true;
            animator.SetBool("IsDeath", isDeath);
        }
        if (!isDeath)//非死亡状态执行
        {
            //获取移动坐标
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))//切换跑步/行走
            {
                isRun = true;
                animator.SetBool("IsRun", true);
            }
            else
            {
                isRun = false;
                animator.SetBool("IsRun", false);
            }

            if (isRun)//逻辑错误造成BUG
            {
                if (vertical > 0)
                {
                    nextState = CharacterStateType.RunForward;
                }
                else if (vertical < 0)
                {
                    nextState = CharacterStateType.RunBackward;
                }
                else
                {
                    nextState = CharacterStateType.Idle;
                }
            }
            else
            {
                if (horizontal > 0)
                {
                    nextState = CharacterStateType.StrafeRight;
                }
                else if (horizontal < 0)
                {
                    nextState = CharacterStateType.StrafeLeft;
                }

                if (vertical > 0)
                {
                    nextState = CharacterStateType.WalkForward;
                }
                else if (vertical < 0)
                {
                    nextState = CharacterStateType.WalkBackward;
                }
            }
            //利用+bool解决多重判断
            if (Input.GetKey(KeyCode.Q))
            {
                animator.SetBool("IsTurnLeft", true);
            }
            else
            {
                animator.SetBool("IsTurnLeft", false);
            }
            //利用bool解决多重判断
            if (Input.GetKey(KeyCode.E))
            {
                animator.SetBool("IsTurnRight", true);
            }
            else
            {
                animator.SetBool("IsTurnRight", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))//跳跃
            {
                if (isRun)//跑步跳
                {
                    animator.SetTrigger("RunJump");
                }
                else//走路跳
                {
                    animator.SetTrigger("Jump");
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                animator.SetTrigger("Gather");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                animator.SetTrigger("Mining");
            }

            if (animator.GetBool("IsCombat"))
            {
                curState = CharacterStateType.C_Idle;
            }
            //战斗部分
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                animator.SetTrigger("PunchLeft");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                animator.SetTrigger("PunchRight");
            }

            if (Input.GetKey(KeyCode.Alpha3))
            {
                animator.SetBool("BlockingLoop",true);
            }
            else
            {
                animator.SetBool("BlockingLoop", false);
            }



        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                nextState = CharacterStateType.Buff;
                isDeath = false;
                animator.SetBool("IsDeath", isDeath);
            }
        }

        if (nextState != curState)
        {
            curState = nextState;
            switch (nextState)
            {
                case CharacterStateType.Idle:
                case CharacterStateType.WalkForward:
                case CharacterStateType.WalkBackward:
                case CharacterStateType.StrafeLeft:
                case CharacterStateType.StrafeRight:
                //case CharacterStateType.TurnLeft://变开关
                //case CharacterStateType.TurnRight:
                case CharacterStateType.RunForward:
                case CharacterStateType.RunBackward:
                //case CharacterStateType.RunJump://变Trigger，不进入状态切换
                //case CharacterStateType.Jump:
                case CharacterStateType.StunnedLoop:
                case CharacterStateType.Death:
                case CharacterStateType.Buff:
                case CharacterStateType.Hello:
                case CharacterStateType.Talk:
                case CharacterStateType.Listen:

                    //case CharacterStateType.Gather:
                    //case CharacterStateType.Mining:
                    animator.SetInteger("State", (int)nextState);
                    break;
                default:
                    break;
            }
        }
    }
}

