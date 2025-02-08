using UnityEngine;

namespace KrazyKatGames
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        public float health = 100f;
        public float maxHealth = 100f;
        public HealthSlider healthSlider;

        [Header("Movement Settings")]
        public float forwardSpeed = 4f; // Speed when typing forward
        public float backwardSpeed = 1f; // Speed when deleting characters

        [Header("Force Settings")]
        public Vector2 pushAnimationForce = new(5f, 0f);

        private Rigidbody2D playerRigidbody; // Reference to the Rigidbody2D
        private Animator playerAnimator; // Reference to the Animator
        private bool playAlternateAnimation; // Toggle between animations
        private float currentSpeed; // Store the current movement speed to be applied in LateUpdate
        public bool isPerformingAction;

        private void Awake()
        {
            playerRigidbody = GetComponent<Rigidbody2D>();
            playerAnimator = GetComponent<Animator>();

            if (playerRigidbody == null || playerAnimator == null)
            {
                Debug.LogError("Rigidbody2D or Animator component is missing on the player GameObject.");
            }
            
            healthSlider.SetMaxHealth(Mathf.RoundToInt(maxHealth));
            healthSlider.SetHealth(Mathf.RoundToInt(health));
        }

        private void LateUpdate()
        {
            // Apply movement to Rigidbody in LateUpdate
            if (playerRigidbody != null && !isPerformingAction)
            {
                Vector2 targetVelocity = new Vector2(currentSpeed, playerRigidbody.linearVelocity.y);
                playerRigidbody.linearVelocity = Vector2.Lerp(playerRigidbody.linearVelocity, targetVelocity, Time.deltaTime * 5f);
            }
        }
        public void TakeDamage(float damage)
        {
            health -= damage;
            
            healthSlider.SetHealth(Mathf.RoundToInt(health));
            
            if (health <= 0)
            {
                playerAnimator.SetBool("IsDead", true);
            }
            else
            {
                playerAnimator.CrossFade("SkaterGirl_Hit_Obstacle", 0.1f);
                isPerformingAction = true;
            }
        }
        public void UpdateMovement(float moveSpeed)
        {
            currentSpeed = moveSpeed;
        }

        public void UpdateAnimatorBlend(float blendValue)
        {
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", blendValue);
            }
        }

        public void PlayWhitespaceAnimation()
        {
            if (isPerformingAction) return;

            if (playerAnimator != null)
            {
                isPerformingAction = true;
                playerAnimator.CrossFade("Skater_Girl_PushSingle", 0.1f);
                ApplyForceImpulse(pushAnimationForce);
            }
        }

        public void PlayEndOfSentenceAnimation()
        {
            if (isPerformingAction) return;

            if (playerAnimator != null)
            {
                isPerformingAction = true;
                playerAnimator.CrossFade("Skater_Girl_PushDouble", 0.1f);
                ApplyForceImpulse(pushAnimationForce);
            }
        }

        public void ApplyForceImpulse(Vector2 impulse, ForceMode2D mode = ForceMode2D.Impulse)
        {
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(impulse, mode);
            }
        }
        public void ApplyAnimationImpulse()
        {
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(pushAnimationForce, ForceMode2D.Impulse);
            }
        }
    }
}