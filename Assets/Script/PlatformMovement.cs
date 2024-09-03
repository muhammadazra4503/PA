using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Platform")]
    [SerializeField] private Transform platform;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale;
    private bool movingLeft;

    private void Awake()
    {
        if (platform != null)
        {
            initScale = platform.localScale;
        }
    }

    private void Update()
    {
        if (platform == null) return;  // Prevent null reference

        if (movingLeft)
        {
            if (platform.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (platform.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else        
                DirectionChange();
        }
    }

    private void DirectionChange()
    {
        movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        platform.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);
        platform.position = new Vector3(platform.position.x + Time.deltaTime * _direction * speed, 
        platform.position.y, platform.position.z);
    }
}
