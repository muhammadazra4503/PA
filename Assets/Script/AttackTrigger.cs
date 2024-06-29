using System;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{
    public event Action<Collider> OnPlayerEnter;
    public event Action<Collider> OnPlayerStay;
    public event Action<Collider> OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerEnter?.Invoke(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerStay?.Invoke(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnPlayerExit?.Invoke(other);
        }
    }
}
