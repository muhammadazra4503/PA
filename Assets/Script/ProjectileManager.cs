using System.Collections;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DisableProjectile(GameObject projectile, float delay)
    {
        StartCoroutine(DisableAfterDelay(projectile, delay));
    }

    private IEnumerator DisableAfterDelay(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        projectile.SetActive(false);
    }
}
