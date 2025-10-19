using UnityEngine;
using System;

public class PlayerHands : MonoBehaviour
{
    public static event Action<int> OnDoorTouched; // enviamos número de puerta
    public static event Action OnDoorExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door1"))
        {
            Debug.Log("Player touched Door 1");
            OnDoorTouched?.Invoke(1);
        }
        else if (other.CompareTag("Door2"))
        {
            Debug.Log("Player touched Door 2");
            OnDoorTouched?.Invoke(2);
        }
        else if (other.CompareTag("Door3"))
        {
            Debug.Log("Player touched Door 3");
            OnDoorTouched?.Invoke(3);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door1") || other.CompareTag("Door2") || other.CompareTag("Door3"))
        {
            Debug.Log("Player left door area");
            OnDoorExit?.Invoke();
        }
    }
}
