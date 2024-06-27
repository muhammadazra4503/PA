using System;
using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private float _direction;
    private float _timer;
    private bool _hit;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetDirection(float direction)
    {
        _direction = direction;
        _timer = 0f;
        _hit = false;
        gameObject.SetActive(true);
        GetComponent<BoxCollider>().enabled = true;

        // Adjust the scale based on the direction
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);

        // Set the initial velocity
        _rigidbody.velocity = new Vector3(_direction * speed, 0, 0);

        Debug.Log("Direction set to: " + _direction);
    }

    private void Update()
    {
        if (!_hit)
        {
            _timer += Time.deltaTime;

            Debug.Log("Projectile moving. Direction: " + _direction + ", Speed: " + speed + ", Timer: " + _timer);

            if (_timer >= lifetime)
            {
                gameObject.SetActive(false);
                Debug.Log("Projectile lifetime ended.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _hit = true;
            GetComponent<BoxCollider>().enabled = false;
            Debug.Log("Projectile hit: " + other.gameObject.name);

            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1); // Assuming 1 damage per hit
                ProjectileManager.Instance.DisableProjectile(gameObject, 0.1f);
            }
        }
    }

    public void OnHit()
    {
        _hit = true;
        GetComponent<BoxCollider>().enabled = false;
        gameObject.SetActive(false);
        Debug.Log("Projectile hit.");
    }

    internal void SetTarget(GameObject gameObject)
    {
        throw new NotImplementedException();
    }
}
