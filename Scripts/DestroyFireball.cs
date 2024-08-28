using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyFireball : MonoBehaviour
{
    private Animator animator;
    private bool hit;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        Invoke("LateDestroy", 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) return;

        hit = true;
        animator.Play("Burn");
        Invoke("LateDestroy", 3);
        transform.parent = collision.transform;
        transform.localRotation = Quaternion.identity;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        if (collision.transform.gameObject.layer == 8)
        {
            transform.localPosition = Vector2.zero;
        }
    }

    private void LateDestroy()
    {
        Destroy(gameObject);
    }
}
