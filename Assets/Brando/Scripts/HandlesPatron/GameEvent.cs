

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/GameEvent", order = 1)]
public class GameEvent : ScriptableObject
{
    private List<GameEventListeners> gameListeners;
    private void OnEnable()
    {
        gameListeners = new();
    }
    private void OnDisable()
    {
        gameListeners.Clear();
    }
    public void Raise()
    {
        foreach (var listener in gameListeners)
        {
            listener.OnRaiseNotified();
        }
    }
    public void RegistryListaner(GameEventListeners gameListener)
    {
        gameListeners.Add(gameListener);
    }
    public void UnRegistryListaner(GameEventListeners gameListener)
    {
        if (gameListeners.Contains(gameListener))
        {
            gameListeners.Remove(gameListener);
        }
    }
}
