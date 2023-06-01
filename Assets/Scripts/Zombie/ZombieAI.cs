using UnityEngine;


public class ZombieAI : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float speed = 2f; // Zombie's movement speed
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


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        zombieTransform = transform;
        originalRotation = zombieTransform.rotation;
    }

    private void Update()
    {
        if (player != null)
        {
            // Calculate the direction to the player
            Vector2 direction = player.position - zombieTransform.position;
            direction.Normalize();

            // Check if the zombie can attack
            if (!isAttacking && Time.time >= nextAttackTime && Vector2.Distance(zombieTransform.position, player.position) <= attackRange)
            {
                Attack();
            }

            // Move the zombie towards the player if not attacking
            if (!isAttacking)
            {
                rb.velocity = direction * speed;

                // Rotate the zombie to face the player
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                zombieTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            // Check if the attack animation has ended
            if (isAttacking && Time.time >= attackEndTime)
            {
                isAttacking = false;
                rb.velocity = Vector2.zero; // Stop the zombie's movement after the attack
                rb.isKinematic = false; // Allow the zombie to be affected by physics again
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

    public void SetPlayer(Transform newPlayer)
    {
        player = newPlayer;
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
        // Handle death logic here (e.g., play death animation, destroy the zombie GameObject, etc.)
        Destroy(gameObject);
    }
}
