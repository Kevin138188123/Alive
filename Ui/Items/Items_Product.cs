using System.Collections.Generic;
using UnityEngine;
/*
��Ʒ��Ʒ��
*/


//���߻���
public class Items_Product
{
    //��ͬ��Ʒ�в�ͬö�٣���������ʹ���������ÿ�������
    public enum ItemType
    {
        None = 0,
        IT_Drug,
        IT_Clutter,
        IT_Equip,
    }

    public enum StateType
    { 
        None=0,
        ST_InBag,
        ST_InEquip,
    }

    int id;
    ItemType item_Type;
    StateType state_Type;
    string typeName;
    string itemName;
    string iconPath;
    int num;
    int maxNum;
    int buyPrice;
    int sellPrice;
    string desc;

    public int Id { get => id; set => id = value; }
    public ItemType Item_Type { get => item_Type; set => item_Type = value; }
    public string TypeName { get => typeName; set => typeName = value; }
    public string ItemName { get => itemName; set => itemName = value; }
    public string IconPath { get => iconPath; set => iconPath = value; }
    public int MaxNum { get => maxNum; set => maxNum = value; }
    public int BuyPrice { get => buyPrice; set => buyPrice = value; }
    public int SellPrice { get => sellPrice; set => sellPrice = value; }
    public string Desc { get => desc; set => desc = value; }
    public int Num { get => num; set => num = value; }
    public StateType State_Type { get => state_Type; set => state_Type = value; }

    public Items_Product() { }

    //�������죬���ο���
    public Items_Product(Items_Product itemPro)
    {
        //itemPro.Id=Id;
        Id = itemPro.Id;
        Item_Type = itemPro.Item_Type;
        TypeName = itemPro.TypeName;
        ItemName = itemPro.ItemName;
        IconPath = itemPro.IconPath;
        MaxNum = itemPro.MaxNum;
        BuyPrice = itemPro.BuyPrice;
        SellPrice = itemPro.SellPrice;
        Desc = itemPro.Desc;
        Num= itemPro.Num;
        State_Type = itemPro.State_Type;
    }

    //��¡�������������ÿ���
    public virtual Items_Product Clone()
    {
        Items_Product itemPro = new Items_Product();//new����ʱ����빹�캯�������¶��󣬶��¶���ֵ��ɿ�¡
        itemPro.id = id;
        itemPro.Item_Type = Item_Type;
        itemPro.TypeName = TypeName;
        itemPro.ItemName = ItemName;
        itemPro.buyPrice = buyPrice;
        itemPro.sellPrice = sellPrice;
        itemPro.maxNum = maxNum;
        itemPro.num = num;
        itemPro.desc = desc;
        itemPro.iconPath = iconPath;
        itemPro.state_Type = state_Type;
        return itemPro;
    }
}

//ҩ��ҩƷ�࣬������ҩˮ�Լ�hp��mpҩˮ
public class Drug_Product : Items_Product
{
    public enum DrugType
    {
        None = 0,
        DT_Hp,
        DT_Mp,
        DT_HpBuff,
        DT_MpBuff,
        DT_AtkBuff,
        DT_DefBuff,
    }

    int hp;
    int mp;
    int maxHp;
    int maxMp;
    int atk;
    int def;
    int buffTime;
    DrugType drug_Type;

    public int Hp { get => hp; set => hp = value; }
    public int Mp { get => mp; set => mp = value; }
    public DrugType Drug_Type { get => drug_Type; set => drug_Type = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int MaxMp { get => maxMp; set => maxMp = value; }
    public int Atk { get => atk; set => atk = value; }
    public int Def { get => def; set => def = value; }
    public int BuffTime { get => buffTime; set => buffTime = value; }

    public Drug_Product() { }

    public Drug_Product(Drug_Product drugPro) : base(drugPro)
    {
        Hp= drugPro.Hp;
        Mp= drugPro.Mp;
        MaxHp = drugPro.MaxHp;
        MaxMp = drugPro.MaxMp;
        Atk = drugPro.Atk;
        Def = drugPro.Def;
        BuffTime= drugPro.BuffTime;
        Drug_Type = drugPro.Drug_Type;
    }

    public override Items_Product Clone()
    {
        return new Drug_Product(this);
    }
}

//װ����
public class Equip_Product : Items_Product
{
    public enum EquipType
    {
        None = 0,
        ET_Weapons,
        ET_Shields,
        ET_Helmets,
        ET_Shoulders,
        ET_Armor,
        ET_Bracers,
        ET_Gloves,
        ET_Belts,
        ET_Pants,
        ET_Boots,
    }

    int atk;
    int def;
    int maxHp;
    int maxMp;
    EquipType equip_Type;

    public int Atk { get => atk; set => atk = value; }
    public int Def { get => def; set => def = value; }
    public EquipType Equip_Type { get => equip_Type; set => equip_Type = value; }
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int MaxMp { get => maxMp; set => maxMp = value; }

    public Equip_Product() { }

    public Equip_Product(Equip_Product equipPro) : base(equipPro)
    {
        Atk = equipPro.Atk;
        Def = equipPro.Def;
        MaxHp = equipPro.MaxHp;
        MaxMp = equipPro.MaxMp;
        Equip_Type = equipPro.Equip_Type;
    }

    public override Items_Product Clone()
    {
        return new Equip_Product(this);
    }
}

//�����࣬����ʳ��⣬���ֲ������ϣ���ʯ��ľ�ϣ�������Ʒ��
public class Clutter_Product:Items_Product
{
    public enum ClutterType
    { 
        None=0,
        CT_Food,
        CT_Material,
    }

    ClutterType clutter_Type;
    //����������ز�Ʒ��Ϣ

    public ClutterType Clutter_Type { get => clutter_Type; set => clutter_Type = value; }
    public Clutter_Product() { }

    public Clutter_Product(Clutter_Product clutterPro) : base(clutterPro)
    {
        Clutter_Type = clutterPro.Clutter_Type;
    }

    public override Items_Product Clone()
    {
        return new Clutter_Product(this);
    }
}
