using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatformMovement : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform topEdge;
    [SerializeField] private Transform bottomEdge;

    [Header("Platform")]
    [SerializeField] private Transform platform;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float idleDuration = 2f;  // Durasi idle
    private Vector3 initScale;
    private bool movingUp;
    private bool isIdle;

    private void Awake()
    {
        if (platform != null)
        {
            initScale = platform.localScale;
        }
    }

    private void Update()
    {
        if (platform == null || isIdle) return;  // Cegah null reference dan idle check

        if (movingUp)
        {
            if (platform.position.y <= topEdge.position.y)
                MoveInDirection(1);
            else
                StartCoroutine(IdleAndChangeDirection());
        }
        else
        {
            if (platform.position.y >= bottomEdge.position.y)
                MoveInDirection(-1);
            else        
                StartCoroutine(IdleAndChangeDirection());
        }
    }

    private IEnumerator IdleAndChangeDirection()
    {
        isIdle = true;  // Set platform to idle
        yield return new WaitForSeconds(idleDuration);  // Tunggu selama idleDuration
        DirectionChange();  // Ubah arah setelah idle
        isIdle = false;  // Keluar dari idle mode
    }

    private void DirectionChange()
    {
        movingUp = !movingUp;
    }

    private void MoveInDirection(int _direction)
    {
        platform.localScale = new Vector3(initScale.x, Mathf.Abs(initScale.y) * _direction, initScale.z);
        platform.position = new Vector3(platform.position.x, 
                                        platform.position.y + Time.deltaTime * _direction * speed, 
                                        platform.position.z);
    }
}
