using System.Collections;
using Code.Weapons;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    [SerializeField] private Transform target; // Reference to the target prefab's transform

    private NavMeshAgent _navMeshAgent;
    
    public int maxHealth = 100; // Maximum health of the zombie
    public float attackRange = 1.5f; // Range of the zombie's attack
    public int attackDamage = 10; // Damage inflicted by the zombie's attack
    public float attackCooldown = 2f; // Cooldown between zombie's attacks
    public float attackDuration = 1f; // Duration of the zombie's attack animation

    public Animator animator; // Reference to the zombie's Animator component

    [SerializeField] private int currentHealth; // Current health of the zombie
    private Rigidbody2D rb;
    private Transform zombieTransform;
    private float nextAttackTime; // Time of the next available attack
    private bool isAttacking; // Flag indicating if the zombie is currently attacking
    private float attackEndTime; // Time when the attack animation should end
    private Quaternion originalRotation; // Original rotation of the zombie before the attack
    
    private ZombieRoundManager roundManager; // Reference to the ZombieRoundManager component

    [Header("Currency")]
    public int currencyOnKill = 10; // Amount of currency to award per zombie kill
    private CurrencyManager currencyManager;
    
    [SerializeField] private AudioClip[] _attackSounds; // Array of sound effects for attacking
    private AudioSource _audioSource; // Reference to the Audio Source component

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        zombieTransform = transform;
        originalRotation = zombieTransform.rotation;
        
        currencyManager = FindObjectOfType<CurrencyManager>();
        roundManager = FindObjectOfType<ZombieRoundManager>();

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateUpAxis = false;
        _navMeshAgent.updateRotation = false;
        
        _audioSource = GetComponent<AudioSource>();
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("ZombieAI: No player object found with tag 'Player'. Make sure to assign the player object or tag it correctly.");
        }
    }

    private void Update()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector2 direction = target.position - zombieTransform.position;
            direction.Normalize();

            // Check if the zombie can attack
            if (!isAttacking && Time.time >= nextAttackTime && Vector2.Distance(zombieTransform.position, target.position) <= attackRange)
            {
                Attack();
            }

            // Move the zombie towards the target if not attacking
            if (!isAttacking)
            {
                _navMeshAgent.SetDestination(target.position);

                // Rotate the zombie to face the target
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                zombieTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // Check if the attack animation has ended
            if (isAttacking && Time.time >= attackEndTime)
            {
                isAttacking = false;
                rb.velocity = Vector2.zero;
                rb.isKinematic = false;
            }
        }
    }

    private void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;
        
        // Store the original rotation before the attack
        originalRotation = zombieTransform.rotation;

        // Trigger the attack animation
        animator.SetTrigger("Attack");

        // Set the flag for attacking and the attack end time
        isAttacking = true;
        attackEndTime = Time.time + attackDuration;

        // Detect colliders within the attack area
        Collider2D[] colliders = Physics2D.OverlapCircleAll(zombieTransform.position, attackRange);
        
        AudioClip randomAttackSound = _attackSounds[Random.Range(0, _attackSounds.Length)];
        _audioSource.PlayOneShot(randomAttackSound);

        foreach (Collider2D collider in colliders)
        {
            CharacterHealth characterHealth = collider.GetComponent<CharacterHealth>();

            if (characterHealth != null)
            {
                characterHealth.TakeDamage(attackDamage);
            }
        }
        
        rb.velocity = Vector2.zero; // Stop the zombie's movement during the attack
        rb.isKinematic = true; // Freeze the zombie's position
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void LateUpdate()
    {
        // Restore the original rotation after the attack is finished
        if (isAttacking && Time.time >= attackEndTime)
        {
            isAttacking = false;
            rb.velocity = Vector2.zero; // Stop the zombie's movement after the attack
            zombieTransform.rotation = originalRotation;
        }
    }
    
    private void Die()
    {
        // Call the ZombieKilled() function from the ZombieRoundManager
        roundManager.ZombieKilled();
        currencyManager.AddCurrency(currencyOnKill);
        
        Destroy(gameObject);
    }
}
