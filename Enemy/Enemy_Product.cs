using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;
/*
敌人信息类，定义敌人属性
*/

public class Enemy_Product
{
    public enum EnemyType
    {
        ET_Animal,
        ET_Human,
        ET_Monster,
    }

    int id;
    EnemyType type;
    int level;
    string name;
    int hp;
    int mp;
    int maxHp;
    int maxMp;
    float atk;
    float def;
    float exp;
    int fallId;
    int gold;
    string des;

    public int Id { get => id; set => id = value; }
    public EnemyType Type { get => type; set => type = value; }
    public int Level { get => level; set => level = value; }
    public string Name { get => name; set => name = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int MaxMp { get => maxMp; set => maxMp = value; }
    public float Atk { get => atk; set => atk = value; }
    public float Def { get => def; set => def = value; }
    public string Des { get => des; set => des = value; }
    public float Exp { get => exp; set => exp = value; }
    public int FallId { get => fallId; set => fallId = value; }
    public int Hp { get => hp; set => hp = value; }
    public int Mp { get => mp; set => mp = value; }
    public int Gold { get => gold; set => gold = value; }

    public Enemy_Product() { }

    public Enemy_Product(Enemy_Product enemyPro)
    {
        Id = enemyPro.Id;
        Type = enemyPro.Type;
        Level = enemyPro.Level;
        Name = enemyPro.Name;
        MaxHp = enemyPro.MaxHp;
        MaxMp = enemyPro.MaxMp;
        Atk = enemyPro.Atk;
        Def = enemyPro.Def;
        Exp = enemyPro.Exp;
        FallId = enemyPro.FallId;
        Des = enemyPro.Des;
    }
}

public class Animal_Product : Enemy_Product
{
    public Animal_Product() { }

    public Animal_Product(Animal_Product animalPro):base(animalPro)
    { 
        
    }
}

public class Human_Product:Enemy_Product
{
    public Human_Product() { }

    public Human_Product(Human_Product humanPro):base(humanPro)
    {
        
    }
}

public class Monster_Product : Enemy_Product
{ 
    public Monster_Product() { }

    public Monster_Product(Monster_Product monsterPro):base(monsterPro)
    { 
        
    }
}

