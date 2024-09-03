using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    public Transform player;
    private bool isFacingRight = true;

    void Update()
    {
        // Mengecek posisi player relatif terhadap enemy
        if (player.position.x < transform.position.x && !isFacingRight)
        {
            Flip();
        }
        else if (player.position.x > transform.position.x && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        // Membalikkan arah enemy
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}
