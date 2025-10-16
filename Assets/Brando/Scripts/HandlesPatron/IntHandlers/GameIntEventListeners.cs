using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]

public class GameIntEventListeners 
{
    [SerializeField] private string nameEventListener;
    public GameIntEvent gameIntEvent;
    public UnityEvent<int> responde;
    public void OnEnable()
    {
        gameIntEvent.RegistryListaner(this);
    }
    public void OnDisable()
    {
        gameIntEvent.UnRegistryListaner(this);
    }
    public void OnRaiseNotified(int value)
    {
        responde?.Invoke(value);
    }
}
