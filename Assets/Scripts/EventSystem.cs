using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    START,
    UPDATE,
    FIXED_UPDATE,
    DISTRACTION,
    PLAYER_ATTACKED
}

public static class EventSystem
{
    private static Dictionary<EventType, System.Action> eventRegister = new Dictionary<EventType, System.Action>();

    public static void Subscribe(EventType evt, System.Action func)
    {
        if (!eventRegister.ContainsKey(evt))
        {
            eventRegister.Add(evt, null);
        }

        eventRegister[evt] += func;
    }

    public static void Unsubscribe(EventType evt, System.Action func)
    {
        if (eventRegister.ContainsKey(evt))
        {
            eventRegister[evt] -= func;
        }
    }

    public static void RaiseEvent(EventType evt)
    {
        eventRegister[evt]?.Invoke();
    }
}

public static class EventSystem<T>
{
    private static Dictionary<EventType, System.Action<T>> eventRegister = new Dictionary<EventType, System.Action<T>>();

    public static void Subscribe(EventType evt, System.Action<T> func)
    {
        if (!eventRegister.ContainsKey(evt))
        {
            eventRegister.Add(evt, null);
        }

        eventRegister[evt] += func;
    }

    public static void Unsubscribe(EventType evt, System.Action<T> func)
    {
        if (eventRegister.ContainsKey(evt))
        {
            eventRegister[evt] -= func;
        }
    }

    public static void RaiseEvent(EventType evt, T arg)
    {
        eventRegister[evt]?.Invoke(arg);
    }
}
