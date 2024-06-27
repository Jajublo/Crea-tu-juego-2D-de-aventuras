using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBomb : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("LateDestroy", 5);
    }

    private void LateDestroy()
    {
        Destroy(gameObject);
    }
}
