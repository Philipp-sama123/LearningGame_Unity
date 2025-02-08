using UnityEngine;
using System.Runtime.InteropServices;

namespace KrazyKatGames
{
    public class UnityReactBridge : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void ReactSendMessage(string message);
        public TypingSpeedController typingSpeedController;

        // Method to call from React with this NAME!
        public void ReceiveFromReact(string data)
        {
            Debug.Log($"Data received from React: {data}");
            
            if (data.Contains("Error"))
            {
                Debug.Log($"{data}");

                typingSpeedController.playerController.TakeDamage(10);
            }
            // Do something with the received data, e.g., update game state or variables
        }
        
        public void SendToReact(string message)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                ReactSendMessage(message);
            }
        }
    }
}