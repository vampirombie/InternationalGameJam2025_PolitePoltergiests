using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;

[Serializable]
public class GameEventListeners 
{
    [SerializeField] private string nameEventListener;
    public GameEvent gameEvent;
    public UnityEvent responde;
    public void OnEnable()
    {
        gameEvent.RegistryListaner(this);
    }
    public void OnDisable()
    {
        gameEvent.UnRegistryListaner(this);
    }
    public void OnRaiseNotified()
    {
        responde?.Invoke();
    }

}
