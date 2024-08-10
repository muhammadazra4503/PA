using UnityEngine;
using System.Collections.Generic;

public class ActivateOnDestroy : MonoBehaviour
{
    public List<GameObject> targetObjects; // List target object yang akan dicek
    public GameObject objectToActivate; // Object yang akan diaktifkan setelah semua targetObject terdestroy

    private void Update()
    {
        // Hapus semua target object yang sudah terdestroy dari list
        targetObjects.RemoveAll(target => target == null);

        // Jika semua target object sudah terdestroy, aktifkan objectToActivate
        if (targetObjects.Count == 0 && !objectToActivate.activeSelf)
        {
            objectToActivate.SetActive(true);
            Debug.Log("Semua target object sudah terdestroy. Object lain diaktifkan.");
        }
    }
}
