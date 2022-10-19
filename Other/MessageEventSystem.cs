using System.Collections.Generic;
using UnityEngine;
/*
委托
*/
public class MessageEventSystem 
{
    MessageEventSystem() { }
    public static readonly MessageEventSystem Instance=new MessageEventSystem();

    //创建一个委托类型
    public delegate void MesEvent();
    //创建一个委托字典存放消息和事件
    Dictionary<string, MesEvent> eventLsit=new Dictionary<string, MesEvent>();

    //添加消息和事件，或为一消息添加事件。
    public void AddListener(string _message,MesEvent _func)
    {
        if (eventLsit.ContainsKey(_message))
        {
            eventLsit[_message] += _func;
        }
        else
        {
            eventLsit.Add(_message, _func);
        }
    }

    //删除指定消息的某一事件
    public void DisListener(string _message,MesEvent _func)
    {
        if (eventLsit.ContainsKey(_message))
        {
            eventLsit[_message] -= _func;
        }
    }

    //广播指定的消息，执行对应事件
    public void SendEventMessage(string _message)
    {
        if (eventLsit.ContainsKey(_message))
        {
            eventLsit[_message]();
        }
    }

}

