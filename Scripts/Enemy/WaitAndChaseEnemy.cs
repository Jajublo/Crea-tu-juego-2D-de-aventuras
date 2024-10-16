using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAndChaseEnemy : EnemyHealth
{
    [Header("Global parameters")]
    Animator animator;
    Vector2 direction;
    public bool chasing;

    [Header("RandomPatrol parameters")]
    public float speed;
    public float minPatrolTime;
    public float maxPatrolTime;
    public float minWaitTime;
    public float maxWaitTime;

    [Header("ChasingEnemy parameters")]
    public float chasingSpeed;

    List<Node> path;
    Vector3 destination = Vector3.zero;
    bool destinationReached = true;

    PlayerMovement player;
    public Node[][] grid;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        direction = RandomDirection();
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        player = FindObjectOfType<PlayerMovement>();
    }

    IEnumerator Patrol()
    {
        direction = RandomDirection();
        Animations();
        yield return new WaitForSeconds(Random.Range(minPatrolTime, maxPatrolTime));

        direction = Vector2.zero;
        Animations();
        yield return new WaitForSeconds(Random.Range(minWaitTime, maxWaitTime));

        StartCoroutine(Patrol());
    }

    private Vector2 RandomDirection()
    {
        int x = Random.Range(0, 8);

        return x switch
        {
            0 => Vector2.up,
            1 => Vector2.down,
            2 => Vector2.left,
            3 => Vector2.right,
            4 => new Vector2(1, 1),
            5 => new Vector2(1, -1),
            6 => new Vector2(-1, 1),
            _ => new Vector2(-1, -1),
        };
    }

    private void Update()
    {
        if (!destinationReached && chasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, chasingSpeed * Time.deltaTime);

            if (transform.position == destination)
            {
                destinationReached = true;
                FindNextStep();
            }
        }
    }

    private void FindNextStep()
    {
        if (!chasing) return;

        PathFindingManager.Instance.FindNextStepCoroutine(
            MoveToNextStep, transform.position, player.transform.position, grid);

        CancelInvoke("FindNextStep");
        Invoke("FindNextStep", 0.5f);     
    }

    private void MoveToNextStep(List<Node> path)
    {
        this.path = path;

        if (path == null || path.Count == 0)
        {
            destinationReached = true;
            Invoke("FindNextStep", 0.1f);
        }
        else
        {
            NextInPath();
        }
    }

    private void NextInPath()
    {
        if (path.Count != 0)
        {
            destination = path[path.Count - 1].worldPosition;

            DirectionTowardsDestination();
            Animations();
            destinationReached = false;
        }
        else
        {
            destinationReached = true;
            Invoke("FindNextStep", 0.1f);
        }
    }

    private void DirectionTowardsDestination()
    {
        Vector3 direction = destination - transform.position;

        if (direction == Vector3.zero)
        {
            this.direction = Vector2.zero;
        }

        direction.Normalize();

        Vector2[] possibleDirections = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            new Vector2(1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(-1, -1).normalized
        };

        float maxDot = -Mathf.Infinity;
        int closestIndex = 0;

        for (int i = 0; i < possibleDirections.Length; i++)
        {
            float dot = Vector2.Dot(direction, possibleDirections[i]);
            if (dot > maxDot)
            {
                maxDot = dot;
                closestIndex = i;
            }
        }

        this.direction = possibleDirections[closestIndex];
    }

    private void Animations()
    {
        if (direction.magnitude != 0)
        {
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.Play("Run");
        }
        else animator.Play("Idle");

        if (!chasing)
            rigidBody.velocity = direction.normalized * speed;
    }

    public override void StopBehaviour()
    {
        if (!chasing)
        {
            StopAllCoroutines();
        }
        else
        {
            destination = Vector3.zero;
            destinationReached = true;
            CancelInvoke("FindNextStep");
        }
        direction = Vector2.zero;
        Animations();
    }

    public override void ContinueBehaviour()
    {
        if (!chasing)
        {
            StartCoroutine(Patrol());
        }
        else
        {
            FindNextStep();
        }
    }

    public override void ResetPosition()
    {
        base.ResetPosition();
        chasing = false;
    }

    private new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Player"))
        {
            StopBehaviour();
            chasing = true;
            ContinueBehaviour();
        }
    }
}
