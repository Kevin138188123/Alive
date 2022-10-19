using PathologicalGames;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
敌人对象池（基类）
*/
public class BearModelPool_Manager : MonoBehaviour
{
    Dictionary<int, GameObject> objects_Lib;
    Dictionary<int, Enemy_Product> objectProModel_Lib;
    public GameObject poolPrefab;
    public int LimitCount;
    public float produceTime;
    float timer;

    #region 单例
    static BearModelPool_Manager instance;
    static readonly object locker = new object();
    public static BearModelPool_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new BearModelPool_Manager();
                    }
                }
            }
            return instance;
        }
    }
    #endregion

    private void Awake()
    {
        instance = this;
        objects_Lib = new Dictionary<int, GameObject>();
        objectProModel_Lib = new Dictionary<int, Enemy_Product>();
        Init();
    }

    private void Update()
    {
        EnableObject();

    }

    public void Init()
    {
        for (int i = 0; i < LimitCount; i++)
        {
            //生成敌人model对应同key预制体   1001手动添加熊
            var enemyPro = Enemy_Factory.Instance.CreateEnemy(1001);
            objectProModel_Lib.Add(i, enemyPro);
            //生成游戏对象对应同keymodel
            var enemy = GameObject.Instantiate(poolPrefab);
            enemy.GetComponent<Bear_Comtrol>().Init(enemyPro, i);
            enemy.transform.SetParent(transform);
            enemy.gameObject.SetActive(true);
            objects_Lib.Add(i, enemy);
        }
    }

    public void EnableObject()
    {
        if (objects_Lib.Count > 0)
        {
            if (timer > produceTime)
            {
                for (int i = 0; i < objects_Lib.Count; i++)
                {
                    if (!objects_Lib[i].activeInHierarchy)
                    {
                        objects_Lib[i].SetActive(true);
                        timer = 0;
                    }
                }
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
        else
        {
            Debug.LogError("敌人字典为空");
        }
    }

    public void Recovery(GameObject _object, float _time)
    {
        StartCoroutine(DeltaDestroy(_object, _time));
    }

    IEnumerator DeltaDestroy(GameObject _object, float _time)
    {
        yield return new WaitForSeconds(_time);
        _object.gameObject.SetActive(false);
    }
}

