using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class GameFloatEventListeners 
{
    [SerializeField] private string nameEventListener;
    public GameFloatEvent gameFloatEvent;
    public UnityEvent<float> responde;
    public void OnEnable()
    {
        gameFloatEvent.RegistryListaner(this);
    }
    public void OnDisable()
    {
        gameFloatEvent.UnRegistryListaner(this);
    }
    public void OnRaiseNotified(float value)
    {
        responde?.Invoke(value);
    }
}
