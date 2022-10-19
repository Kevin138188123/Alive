using System.Collections.Generic;
using UnityEngine;
/*
��ɫ����״̬��
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
    //����
    Hello,
    Talk,
    Listen,
    //�����
    Gather,
    Mining,
    //ս��
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
        if (Input.GetKeyDown(KeyCode.Z))//ģ������
        {
            nextState = CharacterStateType.Death;
            isDeath = true;
            animator.SetBool("IsDeath", isDeath);
        }
        if (!isDeath)//������״ִ̬��
        {
            //��ȡ�ƶ�����
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))//�л��ܲ�/����
            {
                isRun = true;
                animator.SetBool("IsRun", true);
            }
            else
            {
                isRun = false;
                animator.SetBool("IsRun", false);
            }

            if (isRun)//�߼��������BUG
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
            //����+bool��������ж�
            if (Input.GetKey(KeyCode.Q))
            {
                animator.SetBool("IsTurnLeft", true);
            }
            else
            {
                animator.SetBool("IsTurnLeft", false);
            }
            //����bool��������ж�
            if (Input.GetKey(KeyCode.E))
            {
                animator.SetBool("IsTurnRight", true);
            }
            else
            {
                animator.SetBool("IsTurnRight", false);
            }

            if (Input.GetKeyDown(KeyCode.Space))//��Ծ
            {
                if (isRun)//�ܲ���
                {
                    animator.SetTrigger("RunJump");
                }
                else//��·��
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
            //ս������
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
                //case CharacterStateType.TurnLeft://�俪��
                //case CharacterStateType.TurnRight:
                case CharacterStateType.RunForward:
                case CharacterStateType.RunBackward:
                //case CharacterStateType.RunJump://��Trigger��������״̬�л�
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

