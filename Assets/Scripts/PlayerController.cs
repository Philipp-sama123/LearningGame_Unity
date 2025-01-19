using UnityEngine;

namespace KrazyKatGames
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float forwardSpeed = 1f; // Speed when typing forward
        public float backwardSpeed = 0.5f; // Speed when deleting characters

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

        private void Update()
        {
            // We calculate and store the current speed from TypingSpeedController
            // This ensures the typing controller updates the currentSpeed, 
            // and player movement is handled in LateUpdate for Rigidbody
        }

        private void LateUpdate()
        {
            // After all updates, apply the movement in LateUpdate
            if (playerRigidbody != null)
            {
                // Apply the current speed to the Rigidbody in LateUpdate
                Vector2 velocity = new Vector2(currentSpeed, playerRigidbody.velocity.y);
                playerRigidbody.velocity = velocity;
            }
        }

        // Update the player's movement speed (called by TypingSpeedController)
        public void UpdateMovement(float moveSpeed)
        {
            currentSpeed = moveSpeed;
        }

        // Update the animator blend value for the blend tree
        public void UpdateAnimatorBlend(float blendValue)
        {
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("Speed", blendValue);
            }
        }

        // Play alternating acceleration animations
        public void PlayAccelerationAnimation(bool isForward)
        {
            if (isPerformingAction) return;
            if (playerAnimator != null)
            {
                isPerformingAction = true;
                string animationName = playAlternateAnimation ? "Skater_Girl_PushSingle" : "Skater_Girl_PushDouble";
                playAlternateAnimation = !playAlternateAnimation;
                playerAnimator.CrossFade(animationName, 0.1f);
            }
        }
    }
}