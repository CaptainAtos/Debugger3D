using UnityEngine;
using UnityEngine.AI;

public class BugAI : MonoBehaviour
{
    public enum State { Patrol, Chase, Attack }
    public State currentState = State.Patrol;

    public Transform player;
    public float chaseRange = 8f;
    public float attackRange = 1f;
    public float attackCooldown = 1.5f;

    public float patrolRadius = 10f;
    public float patrolWaitTime = 2f;
    public float swarmRadius = 6f;
    public float swarmPullStrength = 0.5f;

    public Animator animator;

    private NavMeshAgent agent;
    private float waitTimer = 0f;
    private float attackTimer = 0f;
    private Vector3 spawnPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (BugSwarmManager.Instance != null)
            BugSwarmManager.Instance.Register(this);

        PickNewPatrolPoint();
    }

    void OnDestroy()
    {
        if (BugSwarmManager.Instance != null)
            BugSwarmManager.Instance.Unregister(this);
    }

    void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (animator != null)
            animator.SetBool("IsMoving", agent.velocity.sqrMagnitude > 0.01f);

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                if (distToPlayer <= chaseRange)
                    currentState = State.Chase;
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                if (distToPlayer <= attackRange)
                    currentState = State.Attack;
                else if (distToPlayer > chaseRange * 1.5f)
                    currentState = State.Patrol;
                break;

            case State.Attack:
                agent.SetDestination(transform.position);
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackCooldown)
                {
                    Attack();
                    attackTimer = 0f;
                }
                if (distToPlayer > attackRange)
                    currentState = State.Chase;
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= patrolWaitTime)
            {
                PickNewPatrolPoint();
                waitTimer = 0f;
            }
        }
    }

    void PickNewPatrolPoint()
    {
        Vector3 center = spawnPosition;
        if (BugSwarmManager.Instance != null)
            center = Vector3.Lerp(spawnPosition, BugSwarmManager.Instance.GetSwarmCenter(this, swarmRadius), swarmPullStrength);

        Vector3 randomOffset = Random.insideUnitSphere * patrolRadius;
        randomOffset.y = 0f;
        Vector3 targetPoint = center + randomOffset;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPoint, out hit, patrolRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void Attack()
    {
        Debug.Log(gameObject.name + " attacks player!");
        if (animator != null)
            animator.SetTrigger("Attack");
    }
}