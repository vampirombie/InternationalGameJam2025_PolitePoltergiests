using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlerEventController : MonoBehaviour
{
    [SerializeField] EventListeners EventListener;
    private void OnEnable()
    {
        for (int i = 0; i < EventListener.SOEvent.Count; i++)
        {
            EventListener.SOEvent[i].OnEnable();
        }
    }
    private void OnDisable()
    {
        for (int i = 0; i < EventListener.SOEvent.Count; i++)
        {
            EventListener.SOEvent[i].OnDisable();
        }
    }
}

[Serializable]
public class EventListeners
{
    public List<GameEventListeners> SOEvent = new ();
}