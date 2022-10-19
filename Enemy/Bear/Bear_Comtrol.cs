using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
�ֿܹ�����
*/
public enum BearStateType
{
    BS_Idle,
    BS_Eat,
    BS_Sleep,
    BS_Sit,
    BS_Patrol,
    BS_Chase,
    BS_Skill_1,
    BS_Skill_2,
    BS_Skill_3,
    BS_Escape,
    BS_Return,
    BS_Death,
}

public class Bear_Comtrol : MonoBehaviour
{
    Enemy_Product enemyPro;//����ʱȷ��������ݺ���Ϣ
    int num;//�����Ԥ�Ƽ��������Ϣ�ı��

    AnimatorStateInfo animatorInfo;//����״̬��״̬�ж�
    NavMeshAgent agent;
    Animator animator;
    GameObject[] player;//ɨ�����
    Transform target;//�������
    BearStateType curState;
    BearStateType nextState;
    Vector3 originalPos;//��ʼλ��
    public string[] skillArr;
    int randomSkill;
    float skill_2_CD;
    bool isCombat;//ս����

    float timer;//��ʱ��
    public float actionTime;//�ж�ʱ��
    Vector3 startChasePos;//׷����ʼ��
    public float waringRadius, chaseRadius, atkRadius;//��Χ�ж�

    float dis_Patrol, dis_ToChase, dis_Atk, dis_Return;//�����ж�


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        curState = BearStateType.BS_Idle;
        agent.Warp(transform.parent.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
        originalPos = transform.position;//��¼��ʼλ��
        //agent.Warp
    }

    //����״ִ̬�ж�Ӧ����Ϊ����������б�
    protected virtual void Update()
    {
        player = GameObject.FindGameObjectsWithTag("Player");
        switch (curState)
        {
            case BearStateType.BS_Idle:
                OnIdle();
                break;
            case BearStateType.BS_Eat:
                OnEat();
                break;
            case BearStateType.BS_Sleep:
            case BearStateType.BS_Sit:
            //�ȴ�״̬ʱ��Ȼ����׷��״̬
            case BearStateType.BS_Patrol:
                OnPatrol();
                break;
            case BearStateType.BS_Chase:
                OnChase();
                break;
            case BearStateType.BS_Skill_1:
            case BearStateType.BS_Skill_2:
                OnAttack();
                break;
            case BearStateType.BS_Escape:
                OnEscape();
                break;
            case BearStateType.BS_Return:
                OnReturn();
                break;
            default:
                break;
        }
        if (curState != nextState)
        {
            curState = nextState;
            animator.SetInteger("State", (int)nextState);
        }
        //�������������
        ToChase();
    }

    public void Init(Enemy_Product _enemyPro, int _num)
    {
        num = _num;
        enemyPro = _enemyPro;
    }

    //׷�����뷶Χ��Ŀ��
    protected virtual void ToChase()
    {
        switch (curState)
        {
            case BearStateType.BS_Chase:
            case BearStateType.BS_Skill_1:
            case BearStateType.BS_Skill_2:
            case BearStateType.BS_Skill_3:
            case BearStateType.BS_Escape:
            case BearStateType.BS_Return:
            case BearStateType.BS_Death:
                return;
        }
        if (player.Length > 0)
        {
            for (int i = 0; i < player.Length; i++)
            {
                dis_ToChase = Vector3.Distance(transform.position, player[i].transform.position);
                if (dis_ToChase < waringRadius)
                {
                    target = player[i].transform;//�ҵ�һ�����뷶Χ�����
                    break;
                }
            }
            if (target != null)//��Ҫ����
            {
                agent.speed = 5;
                nextState = BearStateType.BS_Chase;
                Debug.Log(nextState);
                startChasePos = transform.position;
            }
            else
            {
                agent.speed = 2;
            }
        }
    }

    private void OnEat()
    {
        timer += Time.deltaTime;
        if (timer > actionTime)
        {
            nextState = BearStateType.BS_Idle;
            timer = 0;//����
        }
    }

    Vector3 RandomPos()
    {
        //�������
        Vector2 unitCircle = Random.insideUnitCircle;
        Vector3 direction = new Vector3(unitCircle.x, 0, unitCircle.y);
        //�������
        float length = Random.Range(0, waringRadius);
        Vector3 newPos = originalPos + direction * length;
        return newPos;
    }

    //����״̬
    protected virtual void OnIdle()
    {
        timer += Time.deltaTime;
        if(timer>actionTime)
        {
            //Χ�Ƴ������Զ��������
            agent.destination = RandomPos();
            nextState = BearStateType.BS_Patrol;
            timer = 0;
        }
       
    }

    //Ѳ��
    protected virtual void OnPatrol()
    {
        //�����ֵ
        dis_Patrol = Vector3.Distance(transform.position, agent.destination);
        if (dis_Patrol < 0.1)
        {
            nextState = BearStateType.BS_Eat;
        }
    }

    //׷��Ŀ��
    protected virtual void OnChase()
    {
        agent.destination = target.position - new Vector3(1f, 0, 1f);//���ƽӴ�����
        dis_Atk = Vector3.Distance(transform.position, target.position);
        //�жϹ�������
        if (dis_Atk < atkRadius)
        {
            nextState = BearStateType.BS_Skill_1;
        }
        //�жϷ��ؾ���
        dis_Return = Vector3.Distance(transform.position, startChasePos);
        if (dis_Return - chaseRadius > 0.1)
        {
            nextState = BearStateType.BS_Return;
        }
    }

    protected virtual void OnAttack()
    {
        skill_2_CD -= Time.deltaTime;
        transform.LookAt(target);
        dis_Atk = Vector3.Distance(transform.position, target.position);
        if (dis_Atk > atkRadius)
        {
            nextState = BearStateType.BS_Chase;
        }
        else
        {
            if (skill_2_CD <= 0)
            {
                skill_2_CD = 3;
                animator.SetTrigger("Attack2");
            }
        }
    }

    protected virtual void OnEscape()
    {
        //��Ѫ������
    }
    protected virtual void OnReturn()
    {
        agent.destination = startChasePos;
        dis_Return = Vector3.Distance(transform.position, startChasePos);
        if (dis_Return < 0.1)
        {
            target = null;
            nextState = BearStateType.BS_Idle;
        }

    }

}

