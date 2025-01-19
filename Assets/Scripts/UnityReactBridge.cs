using UnityEngine;
using System.Runtime.InteropServices;

namespace KrazyKatGames
{
    public class UnityReactBridge : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ReactSendMessage(string message);
        public TypingSpeedController typingSpeedController;
        private float timer = 0f;

        void Update()
        {
            // Check if running in WebGL Player and send data every 5 seconds
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                timer += Time.deltaTime;

                if (timer >= 5f)
                {
                    timer = 0f; // Reset the timer

                    // Use the defined ReactSendMessage to send data to React
                    ReactSendMessage($"{typingSpeedController.typingInputField.text}");
                }
            }
        }

        // Method to call from React
        public void ReceiveFromReact(string data)
        {
            Debug.Log($"Data received from React: {data}");
            // Do something with the received data, e.g., update game state or variables
        }

        // Method to manually send data to React (can be called based on Unity events)
        public void SendToReact()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                ReactSendMessage("Manual Hello from Unity!");
            }
        }
    }
}