using System.Collections;
using UnityEngine;
public class FallDetection : MonoBehaviour
{
    // Titik dimana player akan kembali jika jatuh
    public Vector3 respawnPoint;

    // Ketinggian dimana player dianggap jatuh
    public float fallThreshold = -10f;

    void Update()
    {
        // Mengecek jika posisi y player kurang dari fallThreshold
        if (transform.position.y < fallThreshold)
        {
            // Mengembalikan player ke respawnPoint
            transform.position = respawnPoint;
        }
    }
}