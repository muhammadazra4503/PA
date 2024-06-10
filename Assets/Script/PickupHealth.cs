using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHealth : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("HealthPickup"))
    {
        GetComponent<Health>().PickupHealth();
        Destroy(other.gameObject);  // Remove the health pickup from the game
    }
}

}
