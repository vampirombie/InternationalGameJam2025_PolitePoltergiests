using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/GameIntEvent", order = 1)]
public class GameIntEvent : ScriptableObject
{
    private List<GameIntEventListeners> gameListeners;
    private void OnEnable()
    {
        gameListeners = new();
    }
    private void OnDisable()
    {
        gameListeners.Clear();
    }
    public void Raise(int value)
    {
        foreach (var listener in gameListeners)
        {
            listener.OnRaiseNotified(value);
        }
    }
    public void RegistryListaner(GameIntEventListeners gameListener)
    {
        gameListeners.Add(gameListener);
    }
    public void UnRegistryListaner(GameIntEventListeners gameListener)
    {
        if (gameListeners.Contains(gameListener))
        {
            gameListeners.Remove(gameListener);
        }
    }
}
