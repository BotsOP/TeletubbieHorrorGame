using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    START,
    UPDATE,
    FIXED_UPDATE,
    END_GHOST
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
