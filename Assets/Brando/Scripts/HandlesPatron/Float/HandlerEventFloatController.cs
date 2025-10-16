using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerEventFloatController : MonoBehaviour
{
    [SerializeField] EventListenersFloat EventListenersInt;
    private void OnEnable()
    {
        for (int i = 0; i < EventListenersInt.SOEvent.Count; i++)
        {
            EventListenersInt.SOEvent[i].OnEnable();
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < EventListenersInt.SOEvent.Count; i++)
        {
            EventListenersInt.SOEvent[i].OnDisable();
        }
    }
}

[Serializable]
public class EventListenersFloat
{
    public List<GameFloatEventListeners> SOEvent;
}