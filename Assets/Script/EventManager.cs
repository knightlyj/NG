using UnityEngine;
using System.Collections;
using System;

public enum EventId
{
    LocalPlayerCreate, //本地玩家创建
    LocalPlayerDestroy, //本地玩家销毁
    MouseItemChange, //鼠标挂载物品改变
}

public static class EventManager
{
    public delegate void Listener(System.Object sender);
    static Listener[] EventListener;

    static EventManager()
    {
        Array a = Enum.GetValues(typeof(EventId));
        EventListener = new Listener[a.Length];
    }

    public static void AddListener(EventId id, Listener listener)
    {
        EventListener[(int)id] += listener;
    }

    public static void RemoveListener(EventId id, Listener listener)
    {
        EventListener[(int)id] -= listener;
    }

    public static void RaiseEvent(EventId id, System.Object sender)
    {
        if(EventListener[(int)id] != null)
        {
            EventListener[(int)id](sender);
        }
    }
}
