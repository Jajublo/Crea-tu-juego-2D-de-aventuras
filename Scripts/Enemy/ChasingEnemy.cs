using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : EnemyHealth
{
    [Header("ChasingEnemy parameters")]
    public float speed;

    List<Node> path;
    Vector3 destination = Vector3.zero;
    bool destinationReached = true;

    PlayerMovement player;
    public Node[][] grid;

    Animator animator;
    Vector2 direction;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        if (!destinationReached)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (transform.position == destination)
            {
                destinationReached = true;
                FindNextStep();
            }
        }
    }

    private void FindNextStep()
    {
        PathFindingManager.Instance.FindNextStepCoroutine(
            MoveToNextStep, transform.position, player.transform.position, grid);
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

    }

    public override void StopBehaviour()
    {
        destination = Vector3.zero;
        direction = Vector2.zero;
        Animations();
        destinationReached = true;
    }

    public override void ContinueBehaviour()
    {
        FindNextStep();
    }
}
