using UnityEngine;
using UnityEngine.AI;

public class BugAI : MonoBehaviour, IDamageable, IKillable
{
    public enum State { Fall, Patrol, Chase, Attack }

    [SerializeField] private State currentState = State.Patrol;
    [SerializeField] private Animator animator;

    [SerializeField] private float maxHealth = 100;
                     private float currentHealth;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float chaseRange = 8f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private float loseChaseMultiplier = 1.5f;

    [SerializeField] private float patrolRadius = 10f;
    [SerializeField] private float patrolWaitTime = 2f;
    [SerializeField] private float swarmRadius = 6f;
    [SerializeField] private float swarmPullStrength = 0.5f;

    [SerializeField] private float fallSpeed = 5f;

    private float patrolPointReachedDistance = 0.5f;
    private float maxFallDistance = 30f;
    private float groundCheckDistance = 0.5f;
    private float movementThreshold = 0.01f;

    private Transform player;
    private NavMeshAgent agent;
    private float waitTimer = 0f;
    private float attackTimer = 0f;
    private float fallDistance = 0f;
    private Vector3 spawnPosition;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        currentHealth = maxHealth;
        spawnPosition = transform.position;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (BugSwarmManager.Instance != null)
            BugSwarmManager.Instance.Register(this);

        agent.enabled = false;
        currentState = State.Fall;
    }

    void OnDestroy()
    {
        if (BugSwarmManager.Instance != null)
            BugSwarmManager.Instance.Unregister(this);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (animator != null && agent.enabled)
        {
            bool isMoving = agent.velocity.sqrMagnitude > movementThreshold;
            animator.SetBool("IsMoving", isMoving);
        }

        switch (currentState)
        {
            case State.Fall:
                Fall();
                break;

            case State.Patrol:
                Patrol();
                if (distanceToPlayer <= chaseRange)
                    currentState = State.Chase;
                break;

            case State.Chase:
                agent.SetDestination(player.position);
                if (distanceToPlayer <= attackRange)
                    currentState = State.Attack;
                else if (distanceToPlayer > chaseRange * loseChaseMultiplier)
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
                if (distanceToPlayer > attackRange)
                    currentState = State.Chase;
                break;
        }
    }

    void Fall()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        fallDistance += fallSpeed * Time.deltaTime;

        bool foundGround = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        if (foundGround)
        {
            LandAndStartPatrol();
        }
        else if (fallDistance > maxFallDistance)
        {
            Debug.Log(gameObject.name + ": kein Boden gefunden, breche Fallen ab");
            LandAndStartPatrol();
        }
    }

    void LandAndStartPatrol()
    {
        agent.enabled = true;
        agent.Warp(transform.position);
        currentState = State.Patrol;
        PickNewPatrolPoint();
    }

    void Patrol()
    {
        bool reachedDestination = !agent.pathPending && agent.remainingDistance < patrolPointReachedDistance;

        if (reachedDestination)
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
        {
            Vector3 swarmCenter = BugSwarmManager.Instance.GetSwarmCenter(this, swarmRadius);
            center = Vector3.Lerp(spawnPosition, swarmCenter, swarmPullStrength);
        }

        Vector3 randomOffset = Random.insideUnitSphere * patrolRadius;
        randomOffset.y = 0f;
        Vector3 targetPoint = center + randomOffset;

        NavMeshHit hit;
        bool foundPoint = NavMesh.SamplePosition(targetPoint, out hit, patrolRadius, NavMesh.AllAreas);

        if (foundPoint)
        {
            agent.SetDestination(hit.position);
        }
    }

    void Attack()
    {
        if (animator != null)
            animator.SetTrigger("Attack");

        IDamageable playerDamageable = player.GetComponent<IDamageable>();
        if (playerDamageable != null)
            playerDamageable.TakeDamage(damage);
    }

    public void TakeDamage(float dmg) 
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
            Die();
    }

    public void Die() 
    {
        animator.SetTrigger("Die");
            Destroy(gameObject);
    }

    public void Initialize(BugDifficultyTier tier)
    {
        maxHealth = tier.health;
        currentHealth = maxHealth;
        damage = tier.damage;
        agent.speed = tier.speed;
        transform.localScale = Vector3.one * tier.scale;
    }
}
