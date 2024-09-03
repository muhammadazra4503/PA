using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerArea : MonoBehaviour
{
    [Space]
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerStay;
    public UnityEvent OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        OnPlayerEnter?.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        OnPlayerStay?.Invoke();
        // Debug.Log($"Player stay");
    }

    private void OnTriggerExit(Collider other)
    {
        OnPlayerExit?.Invoke();
    }
}
