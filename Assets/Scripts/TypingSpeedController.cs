using UnityEngine;
using TMPro;

namespace KrazyKatGames
{
    public class TypingSpeedController : MonoBehaviour
    {
        public TMP_InputField typingInputField; // Assign your TextMeshPro InputField in the Inspector
        public PlayerController playerController; // Reference to the PlayerController script
        public UnityReactBridge unityReactBridge;
        public float stopDelay = 2.5f; // Time to stop moving after typing stops
        public float typingSpeedThreshold = .1f; // Typing speed threshold for fast mode

        private float lastInputTime; // Time when the last input occurred
        private int previousCharacterCount; // Character count in the InputField last frame
        private float typingSpeed; // Current typing speed
        private bool wasStopped = true;

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

            int currentCharacterCount = typingInputField.text.Length;
            string currentText = typingInputField.text;

            if (currentCharacterCount != previousCharacterCount) // Typing detected
            {
                if (wasStopped)
                {
                    playerController.PlayEndOfSentenceAnimation(); // Play animation when typing resumes
                    wasStopped = false; // Reset the flag
                }

                float timeSinceLastChar = Time.time - lastInputTime;
                lastInputTime = Time.time;
                typingSpeed = timeSinceLastChar > 0 ? 1f / timeSinceLastChar : 0f;

                bool isTypingForward = currentCharacterCount > previousCharacterCount;
                float moveSpeed = isTypingForward ? playerController.forwardSpeed : -playerController.backwardSpeed;
                playerController.UpdateMovement(moveSpeed);

                float blendValue = typingSpeed >= typingSpeedThreshold ? 2f : 1f;
                playerController.UpdateAnimatorBlend(blendValue);

                char lastTypedChar = currentText.Length > 0 ? currentText[currentText.Length - 1] : '\0';

                if (isTypingForward)
                {
                    if (char.IsWhiteSpace(lastTypedChar))
                    {
                        playerController.PlayWhitespaceAnimation();
                    }
                    else if (IsEndOfSentence(lastTypedChar))
                    {
                        playerController.PlayEndOfSentenceAnimation();
                    }
                }
            }
            else if (Time.time - lastInputTime > stopDelay)
            {
                if (!wasStopped)
                {
                    Debug.LogWarning("ERROR - going back!");
                    playerController.UpdateMovement(0f);
                    playerController.UpdateAnimatorBlend(0f);
                    wasStopped = true; // Mark as stopped
                }
            }

            previousCharacterCount = currentCharacterCount;
        }

        // Utility method to detect sentence-ending characters
        private bool IsEndOfSentence(char c)
        {
            unityReactBridge.SendToReact(typingInputField.text);
            return c == '.' || c == '?' || c == '!';
        }
    }
}