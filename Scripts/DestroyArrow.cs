using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyArrow : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("LateDestroy", 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke("LateDestroy", 3);
        transform.parent = collision.transform;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void LateDestroy()
    {
        Destroy(gameObject);
    }
}
