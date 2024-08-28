using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnableObject : MonoBehaviour
{
    private int hp = 3;
    private SpriteRenderer sp;

    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fire"))
        {
            hp--;
            sp.enabled = false;
            Invoke("EnableSpriteRender", 0.1f);
            if (hp <= 0) {
                DataInstance.Instance.SaveSceneData(name);
                Destroy(gameObject);
            }
        }
    }

    private void EnableSpriteRender()
    {
        sp.enabled = true;
    }
}
