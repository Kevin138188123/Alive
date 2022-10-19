using System.Collections.Generic;
using UnityEngine;
/*
ί��
*/
public class MessageEventSystem 
{
    MessageEventSystem() { }
    public static readonly MessageEventSystem Instance=new MessageEventSystem();

    //����һ��ί������
    public delegate void MesEvent();
    //����һ��ί���ֵ�����Ϣ���¼�
    Dictionary<string, MesEvent> eventLsit=new Dictionary<string, MesEvent>();

    //�����Ϣ���¼�����Ϊһ��Ϣ����¼���
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

    //ɾ��ָ����Ϣ��ĳһ�¼�
    public void DisListener(string _message,MesEvent _func)
    {
        if (eventLsit.ContainsKey(_message))
        {
            eventLsit[_message] -= _func;
        }
    }

    //�㲥ָ������Ϣ��ִ�ж�Ӧ�¼�
    public void SendEventMessage(string _message)
    {
        if (eventLsit.ContainsKey(_message))
        {
            eventLsit[_message]();
        }
    }

}

