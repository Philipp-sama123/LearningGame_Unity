using UnityEngine;

namespace KrazyKatGames
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float forwardSpeed = 4f; // Speed when typing forward
        public float backwardSpeed = 1f; // Speed when deleting characters

        [Header("Force Settings")]
        public Vector2 pushSingleForce = new(1f, 0f);
        public Vector2 pushDoubleForce = new(2f, 0f);

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
        }

        private void LateUpdate()
        {
            // Apply movement to Rigidbody in LateUpdate
            if (playerRigidbody != null)
            {
                Vector2 targetVelocity;

                // If no force is being applied, we want to maintain a constant speed
                if (!isPerformingAction)
                {
                    targetVelocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
                }
                else
                {
                    // If a force is applied, we preserve the existing velocity but apply the force's influence
                    targetVelocity = new Vector2(playerRigidbody.velocity.x, playerRigidbody.velocity.y);
                }

                // Apply the target velocity
                playerRigidbody.velocity = targetVelocity;
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

        // Play animation for whitespace
        public void PlayWhitespaceAnimation()
        {
            if (isPerformingAction) return;

            if (playerAnimator != null)
            {
                isPerformingAction = true;

                playerAnimator.CrossFade("Skater_Girl_PushSingle", 0.1f);
                ApplyForceImpulse(pushSingleForce);
            }
        }

        // Play animation for end of sentence
        public void PlayEndOfSentenceAnimation()
        {
            if (isPerformingAction) return;

            if (playerAnimator != null)
            {
                isPerformingAction = true;

                playerAnimator.CrossFade("Skater_Girl_PushDouble", 0.1f);
                ApplyForceImpulse(pushDoubleForce);
            }
        }
        public void ApplyForceImpulse(Vector2 impulse, ForceMode2D mode = ForceMode2D.Impulse)
        {
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(impulse, mode);
            }
        }
    }
}