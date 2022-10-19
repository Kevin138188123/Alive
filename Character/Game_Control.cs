using System.Collections.Generic;
using UnityEngine;
/*
��Ϸ������
*/
public class Game_Control 
{
    #region ����
    static Game_Control() { }
    static Game_Control instance;
    static object locker = new object();
    public static Game_Control Instance
    {
        get
        {
            if (instance == null)
            {
                lock (locker)
                {
                    if (instance == null)
                    {
                        instance = new Game_Control();
                    }
                }

            }
            return instance;
        }
    }
    #endregion

    public bool isDeath;
    public bool isCombat;


}

