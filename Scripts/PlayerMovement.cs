using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public SpriteRenderer pickItem;
    Vector2 direction;

    Rigidbody2D rigidBody;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool isAttacking;
    public GameObject arrow;
    public GameObject bomb;

    bool invincible;
    float invincibilityTime = 1.2f;
    float blinkTime = 0.1f;

    bool uncontrollable;
    float knockbackStrength = 2f;
    float knockbackTime = 0.3f;

    GameManager gameManager;

    List<BasicInteraction> basicInteractionList = new List<BasicInteraction>();

    private void Awake()
    {
        transform.position = DataInstance.Instance.playerPosition;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if(!uncontrollable)
        {
            rigidBody.velocity = direction * speed;
        }
    }

    private void Update()
    {
        Inputs();
        Animations();
    }

    private void Inputs()
    {
        if (isAttacking || uncontrollable || Time.timeScale == 0) return;

        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (basicInteractionList != null)
            {
                Vector2 playerFacing = new Vector2(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
                bool interactionSuccess = false;

                foreach (BasicInteraction basicInteraction in basicInteractionList)
                {
                    if (interactionSuccess) return;
                    if (basicInteraction.Interact(playerFacing, transform.position))
                    {
                        interactionSuccess = true;
                    }
                }

                if (!interactionSuccess)
                {
                    Attack();
                }
            }

            else
            {
                Attack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Bow();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Bomb();
        }
    }

    private void Attack()
    {
        animator.Play("Attack");
        isAttacking = true;
        AttackAnimDirection();
    }

    private void Bow()
    {
        animator.Play("Bow");
        isAttacking = true;
        AttackAnimDirection();
    }

    private void Shoot()
    {
        var obj = Instantiate(arrow);
        Vector2 direction = new Vector2(animator.GetFloat("Horizontal"), animator.GetFloat("Vertical"));
        obj.transform.eulerAngles = new Vector3(0, 0, Vector2.SignedAngle(new Vector2(direction.x , -direction.y), Vector2.right));
        obj.transform.position = transform.position + new Vector3(direction.x, 0, direction.y) * 0.2f;
        obj.GetComponent<Rigidbody2D>().velocity = direction * 10;
    }

    private void Bomb()
    {
        var obj = Instantiate(bomb, transform.position, Quaternion.identity);
    }

    private void Animations()
    {
        if (isAttacking || uncontrollable || Time.timeScale == 0) return;

        if (direction.magnitude != 0)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.Play("Run");
        }
        else animator.Play("Idle");
    }

    private void AttackAnimDirection()
    {
        direction.x = animator.GetFloat("Horizontal");
        direction.y = animator.GetFloat("Vertical");

        if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction = direction.normalized;

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        direction = Vector2.zero;
    }

    private void EndAttack()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MaxHpUp"))
        {
            Destroy(collision.gameObject);
            gameManager.IncreaseMaxHP();
            pickItem.sprite = collision.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(PickItem());
            DataInstance.Instance.SaveSceneData(collision.name);
        }
        else if (collision.CompareTag("Heal") && gameManager.CanHeal())
        {
            Destroy(collision.gameObject);
            gameManager.UpdateCurrentHP(4);
        }
        else if (collision.CompareTag("Interaction"))
        {
            basicInteractionList.Add(collision.GetComponent<BasicInteraction>());
        }
        else if (collision.CompareTag("Key"))
        {
            Destroy(collision.gameObject);
            gameManager.UpdateCurrentKeys(1);
            pickItem.sprite = collision.GetComponent<SpriteRenderer>().sprite;
            StartCoroutine(PickItem());
            DataInstance.Instance.SaveSceneData(collision.name);
        }
        else if (collision.CompareTag("Explosion") && !invincible)
        {
            gameManager.UpdateCurrentHP(-2);
            StartCoroutine(Invincibility());
            StartCoroutine(Knockback(collision.transform.position));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interaction"))
        {
            basicInteractionList.Remove(collision.GetComponent<BasicInteraction>());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy") && !invincible)
        {
            gameManager.UpdateCurrentHP(-2);
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
        uncontrollable = true;
        direction = Vector2.zero;
        rigidBody.velocity = (transform.position - hitPosition).normalized * knockbackStrength;
        yield return new WaitForSeconds(knockbackTime);
        rigidBody.velocity = Vector3.zero;
        uncontrollable = false;
    }

    IEnumerator PickItem()
    {
        animator.Play("Pick_Item");
        uncontrollable = true;
        direction =rigidBody.velocity = Vector2.zero;
        Camera.main.GetComponent<CameraController>().PauseEnemies();

        yield return new WaitForSeconds(1f);

        uncontrollable = false;
        Camera.main.GetComponent<CameraController>().ResumeEnemies();
    }
}
