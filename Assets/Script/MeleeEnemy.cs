using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private BoxCollider boxCollider; // Use BoxCollider for 3D
    [SerializeField] private float colliderDistance;
    private float cooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Transform player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (PlayerInSight() && cooldownTimer >= attackCooldown)
        {
            cooldownTimer = 0;
            anim.SetTrigger("meleeAttack");
        }
    }

    private bool PlayerInSight()
    {
        if (player == null) return false;

        Vector3 boxColliderCenter = boxCollider.bounds.center;
        Vector3 boxColliderSize = boxCollider.bounds.size;
        Vector3 direction = transform.right * range * transform.localScale.x * colliderDistance;

        RaycastHit hit;
        bool isHit = Physics.BoxCast(boxColliderCenter + direction,
                                     boxColliderSize / 2,
                                     Vector3.left,
                                     out hit,
                                     Quaternion.identity,
                                     0);

        return isHit && hit.collider.CompareTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 boxColliderCenter = boxCollider.bounds.center;
        Vector3 boxColliderSize = boxCollider.bounds.size;
        Vector3 direction = transform.right * range * transform.localScale.x * colliderDistance;

        Gizmos.DrawWireCube(boxColliderCenter + direction, boxColliderSize);
    }
}
