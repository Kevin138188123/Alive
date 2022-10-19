using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
���˹����࣬��������
*/
public class Enemy_Factory
{
    Dictionary<int, Enemy_Product> enemyInfoLib = null;

    Enemy_Factory()
    {
        enemyInfoLib = new Dictionary<int, Enemy_Product>();
        LoadEnemy(AppConst.enemyInfoXmlUrl);
    }
    public static readonly Enemy_Factory Instance = new Enemy_Factory();

    void LoadEnemy(string _fileName)
    {
        if (File.Exists(_fileName))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(_fileName);
            XmlElement root = doc.SelectSingleNode("Root") as XmlElement;
            foreach (XmlElement item in root.ChildNodes)
            {
                Enemy_Product enemyPro = new Enemy_Product();
                //ö������
                Enemy_Product.EnemyType type = (Enemy_Product.EnemyType)int.Parse(item.GetAttribute("type"));
                switch (type)
                {
                    case Enemy_Product.EnemyType.ET_Animal:
                        //��չ����
                        Animal_Product animal_Product = new Animal_Product();
                        enemyPro = animal_Product;
                        break;
                    case Enemy_Product.EnemyType.ET_Human:
                        //��չ����
                        Human_Product human_Product = new Human_Product();
                        enemyPro = human_Product;
                        break;
                    case Enemy_Product.EnemyType.ET_Monster:
                        //��չ����
                        Monster_Product monster_Product = new Monster_Product();
                        enemyPro = monster_Product;
                        break;
                    default:
                        break;
                }
                enemyPro.Id = int.Parse(item.GetAttribute("id"));
                enemyPro.Type = type;
                enemyPro.Level = int.Parse(item.GetAttribute("level"));
                enemyPro.Name = item.GetAttribute("name");
                enemyPro.MaxHp = int.Parse(item.GetAttribute("maxHp"));
                enemyPro.MaxMp = int.Parse(item.GetAttribute("maxMp"));
                enemyPro.Atk = int.Parse(item.GetAttribute("atk"));
                enemyPro.Def = int.Parse(item.GetAttribute("def"));
                enemyPro.Des = item.GetAttribute("des");
                enemyPro.Exp = float.Parse(item.GetAttribute("exp"));
                enemyPro.FallId = int.Parse(item.GetAttribute("fallId"));
                //�����ֵ�
                enemyInfoLib.Add(enemyPro.Id, enemyPro);
            }
        }
        else
        {
            Debug.LogError("δ�ҵ��ļ�" + _fileName);
        }
    }

    public Enemy_Product CreateEnemy(int _id)
    {
        if (enemyInfoLib.ContainsKey(_id))
        {
            return  new Enemy_Product(enemyInfoLib[_id]);
        }
        else
        {
            Debug.LogError("û���ҵ�������Ϣ");
            return null;
        }
    }

}

