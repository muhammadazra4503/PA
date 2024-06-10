using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;

    private float _direction;
    private float _timer;
    private bool _hit;

    public void SetDirection(float direction)
    {
        _direction = direction;
        _timer = 0f;
        _hit = false;
        gameObject.SetActive(true);
        GetComponent<BoxCollider2D>().enabled = true;

        // Adjust the scale based on the direction
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Update()
    {
        if (!_hit)
        {
            transform.Translate(Vector3.right * _direction * speed * Time.deltaTime);
            _timer += Time.deltaTime;

            if (_timer >= lifetime)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Handle collision with other objects
        _hit = true;
        gameObject.SetActive(false);
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
