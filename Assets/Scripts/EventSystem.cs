using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    START,
    UPDATE,
    FIXED_UPDATE,
    DISTRACTION,
    PLAYER_ATTACKED,
    FLASHLIGHT,
    PLAYER_SNEAKING,
    PLAYER_MOVEMENT,
    PLAYER_GROUNDED,
    GAME_WON
}

public static class EventSystem
{
    private static Dictionary<EventType, System.Action> eventRegister = new Dictionary<EventType, System.Action>();

    public static void Subscribe(EventType _evt, System.Action _func)
    {
        if (!eventRegister.ContainsKey(_evt))
        {
            eventRegister.Add(_evt, null);
        }

        eventRegister[_evt] += _func;
    }

    public static void Unsubscribe(EventType _evt, System.Action _func)
    {
        if (eventRegister.ContainsKey(_evt))
        {
            eventRegister[_evt] -= _func;
        }
    }

    public static void RaiseEvent(EventType _evt)
    {
        eventRegister[_evt]?.Invoke();
    }
}

public static class EventSystem<T>
{
    private static Dictionary<EventType, System.Action<T>> eventRegister = new Dictionary<EventType, System.Action<T>>();

    public static void Subscribe(EventType _evt, System.Action<T> _func)
    {
        if (!eventRegister.ContainsKey(_evt))
        {
            eventRegister.Add(_evt, null);
        }

        eventRegister[_evt] += _func;
    }

    public static void Unsubscribe(EventType _evt, System.Action<T> _func)
    {
        if (eventRegister.ContainsKey(_evt))
        {
            eventRegister[_evt] -= _func;
        }
    }

    public static void RaiseEvent(EventType _evt, T _arg)
    {
        eventRegister[_evt]?.Invoke(_arg);
    }
}

public static class EventSystem<A, T>
{
    private static Dictionary<EventType, System.Action<A, T>> eventRegister = new Dictionary<EventType, System.Action<A, T>>();

    public static void Subscribe(EventType _evt, System.Action<A, T> _func)
    {
        if (!eventRegister.ContainsKey(_evt))
        {
            eventRegister.Add(_evt, null);
        }

        eventRegister[_evt] += _func;
    }

    public static void Unsubscribe(EventType _evt, System.Action<A, T> _func)
    {
        if (eventRegister.ContainsKey(_evt))
        {
            eventRegister[_evt] -= _func;
        }
    }

    public static void RaiseEvent(EventType _evt, A _arg,  T _arg2)
    {
        eventRegister[_evt]?.Invoke(_arg, _arg2);
    }
}
