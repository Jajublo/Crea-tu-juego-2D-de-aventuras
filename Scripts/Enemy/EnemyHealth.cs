using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("EnemyHP parameters")]
    public int maxHp = 5;
    public int hp = 5;

    protected bool invincible;
    protected float invincibilityTime = 0.6f;
    protected float blinkTime = 0.1f;  

    public float knockbackStrength = 2f;
    protected float knockbackTime = 0.3f;

    protected Rigidbody2D rigidBody;
    protected SpriteRenderer spriteRenderer;
    protected EnemyHit enemyHit;

    protected Vector2 initialPosition;

    public virtual void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHit = GetComponentInChildren<EnemyHit>();

        initialPosition = transform.position;

        hp = maxHp;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Weapon") || collision.CompareTag("Explosion") || collision.CompareTag("Fire"))
            && !invincible)
        {
            hp--;
            if (hp <= 0)
            {
                enemyHit.Defeat();
                FindObjectOfType<GameManager>().SpawnItem(transform.position);
            }
            StopBehaviour();
            StartCoroutine(Invincibility());
            StartCoroutine(Knockback(collision.transform.position));
        }
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        float auxTime = invincibilityTime;

        while (auxTime > 0)
        {
            yield return new WaitForSeconds(blinkTime);
            auxTime -= blinkTime;
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }

        spriteRenderer.enabled = true;
        invincible = false;
    }

    IEnumerator Knockback(Vector3 hitPosition)
    {
        if (knockbackStrength <= 0)
        {
            if(hp > 0) ContinueBehaviour();
            yield break;
        }

        rigidBody.velocity = (transform.position - hitPosition).normalized * knockbackStrength;
        yield return new WaitForSeconds(knockbackTime);
        rigidBody.velocity = Vector3.zero;
        yield return new WaitForSeconds(knockbackTime);
        if (hp > 0) ContinueBehaviour();
    }

    public void HideEnemy()
    {
        StopAllCoroutines();
        rigidBody.velocity = Vector3.zero;
        spriteRenderer.enabled = false;
    }

    public virtual void StopBehaviour() { }

    public virtual void ContinueBehaviour() { }

    public virtual void ResetPosition()
    {
        transform.position = initialPosition;
        hp = maxHp;
    }
}
