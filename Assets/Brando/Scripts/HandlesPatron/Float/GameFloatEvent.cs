using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObjects/GameFloatEvent", order = 1)]
public class GameFloatEvent : ScriptableObject
{
    private List<GameFloatEventListeners> gameListeners;
    private void OnEnable()
    {
        gameListeners = new();
    }
    private void OnDisable()
    {
        gameListeners.Clear();
    }
    public void Raise(float value)
    {
        foreach (var listener in gameListeners)
        {
            listener.OnRaiseNotified(value);
        }
    }
    public void RegistryListaner(GameFloatEventListeners gameListener)
    {
        gameListeners.Add(gameListener);
    }
    public void UnRegistryListaner(GameFloatEventListeners gameListener)
    {
        if (gameListeners.Contains(gameListener))
        {
            gameListeners.Remove(gameListener);
        }
    }
}
