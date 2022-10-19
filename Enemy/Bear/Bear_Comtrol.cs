using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*
熊怪控制器
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
    Enemy_Product enemyPro;//生成时确定怪物身份和信息
    int num;//对象池预制件与对象信息的编号

    AnimatorStateInfo animatorInfo;//动画状态机状态判断
    NavMeshAgent agent;
    Animator animator;
    GameObject[] player;//扫描玩家
    Transform target;//锁定玩家
    BearStateType curState;
    BearStateType nextState;
    Vector3 originalPos;//初始位置
    public string[] skillArr;
    int randomSkill;
    float skill_2_CD;
    bool isCombat;//战斗中

    float timer;//计时器
    public float actionTime;//行动时间
    Vector3 startChasePos;//追击起始点
    public float waringRadius, chaseRadius, atkRadius;//范围判断

    float dis_Patrol, dis_ToChase, dis_Atk, dis_Return;//距离判断


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        curState = BearStateType.BS_Idle;
        agent.Warp(transform.parent.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
        originalPos = transform.position;//记录初始位置
        //agent.Warp
    }

    //更新状态执行对应的行为、更新玩家列表
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
            //等待状态时让然进入追击状态
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
        //搜索并锁定玩家
        ToChase();
    }

    public void Init(Enemy_Product _enemyPro, int _num)
    {
        num = _num;
        enemyPro = _enemyPro;
    }

    //追击进入范围的目标
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
                    target = player[i].transform;//找第一个进入范围的玩家
                    break;
                }
            }
            if (target != null)//需要重置
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
            timer = 0;//重置
        }
    }

    Vector3 RandomPos()
    {
        //随机方向
        Vector2 unitCircle = Random.insideUnitCircle;
        Vector3 direction = new Vector3(unitCircle.x, 0, unitCircle.y);
        //随机长度
        float length = Random.Range(0, waringRadius);
        Vector3 newPos = originalPos + direction * length;
        return newPos;
    }

    //待机状态
    protected virtual void OnIdle()
    {
        timer += Time.deltaTime;
        if(timer>actionTime)
        {
            //围绕出生点自动随机导航
            agent.destination = RandomPos();
            nextState = BearStateType.BS_Patrol;
            timer = 0;
        }
       
    }

    //巡逻
    protected virtual void OnPatrol()
    {
        //距离差值
        dis_Patrol = Vector3.Distance(transform.position, agent.destination);
        if (dis_Patrol < 0.1)
        {
            nextState = BearStateType.BS_Eat;
        }
    }

    //追击目标
    protected virtual void OnChase()
    {
        agent.destination = target.position - new Vector3(1f, 0, 1f);//限制接触距离
        dis_Atk = Vector3.Distance(transform.position, target.position);
        //判断攻击距离
        if (dis_Atk < atkRadius)
        {
            nextState = BearStateType.BS_Skill_1;
        }
        //判断返回距离
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
        //滴血量逃跑
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

