using UnityEngine;

namespace KrazyKatGames
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class ZombieAI : MonoBehaviour
    {
        [Header("Zombie Settings")]
        public float moveSpeed = 1f; // Speed at which the zombie moves
        public float attackRange = 1f; // Range at which the zombie attacks the player
        public float attackCooldown = 1.5f; // Time between attacks

        private Rigidbody2D zombieRigidbody; // Reference to the Rigidbody2D
        private Animator zombieAnimator; // Reference to the Animator
        [SerializeField] private Transform playerTransform; // Reference to the player's Transform (assigned in Inspector)

        private bool isAttacking; // True if the zombie is attacking
        private float attackTimer; // Tracks time since last attack

        private void Awake()
        {
            zombieRigidbody = GetComponent<Rigidbody2D>();
            zombieAnimator = GetComponent<Animator>();

            if (zombieRigidbody == null || zombieAnimator == null)
            {
                Debug.LogError("Rigidbody2D or Animator component is missing on the zombie GameObject.");
            }
        }

        private void Update()
        {
            if (playerTransform == null) return;

            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRange)
            {
                // Attack the player if in range
                if (!isAttacking)
                {
                    AttackPlayer();
                }

                // Ensure zombie stops completely while attacking
                zombieRigidbody.velocity = Vector2.zero;
                if (zombieAnimator != null)
                {
                    zombieAnimator.SetFloat("Speed", 0f);
                }
            }
            else
            {
                // Move toward the player if not in range
                MoveTowardPlayer();
            }

            // Update the attack timer
            if (isAttacking)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    isAttacking = false; // Ready to attack again
                }

                DetectPlayerInAttackRadius(); // Check for player in attack radius while attacking
            }
        }

        private void MoveTowardPlayer()
        {
            if (isAttacking) return; // Don't move if attacking

            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Vector2 newVelocity = direction * moveSpeed;
            zombieRigidbody.velocity = new Vector2(newVelocity.x, zombieRigidbody.velocity.y);

            // Update the animator with movement speed
            if (zombieAnimator != null)
            {
                zombieAnimator.SetFloat("Speed", Mathf.Abs(newVelocity.magnitude));
            }

            // Flip the zombie to face the player
            if (direction.x > 0 && transform.localScale.x < 0 || direction.x < 0 && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        private void AttackPlayer()
        {
            isAttacking = true;
            attackTimer = attackCooldown;

            zombieRigidbody.velocity = Vector2.zero; // Stop moving while attacking

            // Crossfade the attack animation from an override layer
            if (zombieAnimator != null)
            {
                zombieAnimator.CrossFade("Zombie_Attack_A", 0.1f, 1); // Layer 1 is used for the override layer
            }

            // Here you could add damage logic, such as calling a method on the player's health script
            Debug.Log("Zombie attacks the player!");
        }

        private void DetectPlayerInAttackRadius()
        {
            // Check if the player is still within the attack range during the attack
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange)
            {
                Debug.Log("Player is within attack radius during the attack.");

                // Additional logic for applying damage to the player could be added here
            }
            else
            {
                Debug.Log("Player moved out of attack radius.");
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Draw the attack range as a red wire sphere
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
