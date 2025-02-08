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
        public float damage = 10f;

        private Rigidbody2D zombieRigidbody;
        private Animator zombieAnimator;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private PlayerController player;

        private bool isAttacking;
        private float attackTimer;
        private bool alreadyDamaged; // Ensures damage is applied only once per attack

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
                if (!isAttacking)
                {
                    AttackPlayer();
                }

                zombieRigidbody.linearVelocity = Vector2.zero;

                if (zombieAnimator != null)
                {
                    zombieAnimator.SetFloat("Speed", 0f);
                }
            }
            else
            {
                MoveTowardPlayer();
            }

            if (isAttacking)
            {
                attackTimer -= Time.deltaTime;
                if (attackTimer <= 0)
                {
                    isAttacking = false;
                    alreadyDamaged = false; // Reset the damage flag for the next attack
                }
            }
        }

        private void MoveTowardPlayer()
        {
            if (isAttacking) return;

            Vector2 direction = (playerTransform.position - transform.position).normalized;
            Vector2 newVelocity = direction * moveSpeed;
            zombieRigidbody.linearVelocity = new Vector2(newVelocity.x, zombieRigidbody.linearVelocity.y);

            if (zombieAnimator != null)
            {
                zombieAnimator.SetFloat("Speed", Mathf.Abs(newVelocity.magnitude));
            }

            if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }

        private void AttackPlayer()
        {
            isAttacking = true;
            attackTimer = attackCooldown;

            zombieRigidbody.linearVelocity = Vector2.zero;

            if (zombieAnimator != null)
            {
                zombieAnimator.CrossFade("Zombie_Attack_A", 0.1f, 1); // Trigger attack animation on layer 1
            }

            alreadyDamaged = false; // Reset the damage flag when starting a new attack
        }

        private void DetectPlayerInAttackRadius()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= attackRange && !alreadyDamaged)
            {
                alreadyDamaged = true; // Damage the player only once during the attack
                player.TakeDamage(damage);
                Debug.Log("Zombie damaged the player.");
            }
        }

        // Called during the attack animation via Animation Event
        private void ApplyDamage()
        {
            if (!alreadyDamaged)
            {
                DetectPlayerInAttackRadius();
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
