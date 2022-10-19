using System.Collections.Generic;
using UnityEngine;
/*
玩家信息类
*/
public class Character_Product
{
    public enum CharType
    {
        CT_Defender,
        CT_Melee,
        CT_Archer,
    }

    int id;
    CharType type;
    string accountId;
    int level;
    string typeName;
    string name;
    string species;//种族
    string appellation;//称谓
    string career;//职业
    string organization;//组织
    int hp;
    int mp;
    int maxHp;
    int maxMp;
    int atk;
    int def;
    int exp;
    int maxExp;
    string des;
    int money;

    public int Id { get => id; set => id = value; }
    public CharType Type { get => type; set => type = value; }
    public int Level { get => level; set => level = value; }
    public string Name { get => name; set => name = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int MaxMp { get => maxMp; set => maxMp = value; }
    public int Atk { get => atk; set => atk = value; }
    public int Def { get => def; set => def = value; }
    public int MaxExp { get => maxExp; set => maxExp = value; }
    public string Des { get => des; set => des = value; }
    public string Species { get => species; set => species = value; }
    public string Career { get => career; set => career = value; }
    public string AccountId { get => accountId; set => accountId = value; }
    public string TypeName { get => typeName; set => typeName = value; }
    public string Organization { get => organization; set => organization = value; }
    public int Hp { get => hp; set => hp = value; }
    public int Mp { get => mp; set => mp = value; }
    public int Exp { get => exp; set => exp = value; }
    public string Appellation { get => appellation; set => appellation = value; }
    public int Money { get => money; set => money = value; }

    public Character_Product() { }

    public Character_Product(Character_Product heroPro)
    {
        Id = heroPro.Id;
        AccountId = heroPro.AccountId;
        Type = heroPro.Type;
        Level = heroPro.Level;
        TypeName = heroPro.TypeName;
        Name = heroPro.Name;
        Hp=heroPro.Hp;
        Mp=heroPro.Mp;
        MaxHp = heroPro.MaxHp;
        MaxMp = heroPro.MaxMp;
        Atk = heroPro.Atk;
        Def = heroPro.Def;
        Exp=heroPro.Exp;
        MaxExp = heroPro.MaxExp;
        Des = heroPro.Des;
        Species = heroPro.Species;
        Career = heroPro.Career;
        Organization=heroPro.Organization;
        Appellation=heroPro.Appellation;
        Money=heroPro.Money;
    }

    public virtual Character_Product Clone()
    {
        Character_Product charPro = new Character_Product();
        charPro.id=Id;
        charPro.accountId=AccountId;
        charPro.type=Type;
        charPro.level=Level;
        charPro.TypeName=TypeName;
        charPro.Name=Name;
        charPro.Hp=Hp;
        charPro.Mp=Mp;
        charPro.MaxHp=MaxHp;
        charPro.MaxMp=MaxMp;
        charPro.Atk=Atk;
        charPro.Def=Def;
        charPro.Exp=Exp;
        charPro.MaxExp=MaxExp;
        charPro.Des=Des;
        charPro.Species=Species;
        charPro.Career=Career;
        charPro.Organization=Organization;
        charPro.Appellation=Appellation;
        charPro.Money=Money;
        return charPro;

    }
}

public class Defender_Product : Character_Product
{
    public Defender_Product() { }

    public Defender_Product(Defender_Product defenderPro) : base(defenderPro)
    {

    }

    public override Character_Product Clone()
    {
        return new Defender_Product(this);
    }
}

public class Melee_Product : Character_Product
{
    public Melee_Product() { }

    public Melee_Product(Melee_Product meleePro) : base(meleePro)
    {

    }

    public override Character_Product Clone()
    {
        return new Melee_Product(this);
    }
}

public class Archer_Product : Character_Product
{
    public Archer_Product() { }

    public Archer_Product(Archer_Product archerPro) : base(archerPro)
    {

    }

    public override Character_Product Clone()
    {
        return new Archer_Product(this);
    }
}

