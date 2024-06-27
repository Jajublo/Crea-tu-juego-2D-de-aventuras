using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDummy : MonoBehaviour
{
    public int HP = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") || collision.CompareTag("Explosion"))
        {
            HP--;
            if(HP <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
