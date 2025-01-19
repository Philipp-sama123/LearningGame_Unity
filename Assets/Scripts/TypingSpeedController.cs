using UnityEngine;
using TMPro;

namespace KrazyKatGames
{
    public class TypingSpeedController : MonoBehaviour
    {
        public TMP_InputField typingInputField; // Assign your TextMeshPro InputField in the Inspector
        public PlayerController playerController; // Reference to the PlayerController script
        public float stopDelay = 2.5f; // Time to stop moving after typing stops
        public float typingSpeedThreshold = 2f; // Typing speed threshold for fast mode

        private float lastInputTime; // Time when the last input occurred
        private int previousCharacterCount; // Character count in the InputField last frame
        private float typingSpeed; // Current typing speed

        private void Start()
        {
            lastInputTime = Time.time;
            previousCharacterCount = 0;
            typingSpeed = 0f;

            if (typingInputField != null)
            {
                typingInputField.text = ""; // Ensure the InputField starts empty
            }

            if (playerController == null)
            {
                Debug.LogError("PlayerController is not assigned in TypingSpeedController.");
            }
        }

        private void Update()
        {
            if (typingInputField == null || playerController == null)
                return;

            // Get the current character count
            int currentCharacterCount = typingInputField.text.Length;

            if (currentCharacterCount != previousCharacterCount)
            {
                // Calculate typing speed based on the interval between inputs
                float timeSinceLastChar = Time.time - lastInputTime;
                lastInputTime = Time.time;
                typingSpeed = timeSinceLastChar > 0 ? 1f / timeSinceLastChar : 0f;

                // Determine typing direction
                bool isTypingForward = currentCharacterCount > previousCharacterCount;

                // Notify the PlayerController about the movement and typing speed
                float moveSpeed = isTypingForward ? playerController.forwardSpeed : -playerController.backwardSpeed;
                playerController.UpdateMovement(moveSpeed);

                // Determine blend value (1 for default, 2 for fast) and pass to PlayerController
                float blendValue = typingSpeed >= typingSpeedThreshold ? 2f : 1f;
                playerController.UpdateAnimatorBlend(blendValue);

                // Trigger animation alternation for acceleration
                playerController.PlayAccelerationAnimation(isTypingForward);
            }
            else if (Time.time - lastInputTime > stopDelay)
            {
                // Stop movement if typing stops for the delay duration
                playerController.UpdateMovement(0f);
                playerController.UpdateAnimatorBlend(0f);
            }

            // Update the previous character count
            previousCharacterCount = currentCharacterCount;
        }
    }
}
