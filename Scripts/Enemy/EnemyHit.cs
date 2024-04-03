using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    EnemyHealth enemyHealth;
    Animator animator;

    private void Start()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    public void Defeat()
    {
        animator.Play("Death");
    }

    private void Hide()
    {
        enemyHealth.HideEnemy();
    }

    private void Destroy()
    {
        Destroy(enemyHealth.gameObject);
    }
}
