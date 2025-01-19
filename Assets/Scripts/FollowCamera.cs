using UnityEngine;

namespace KrazyKatGames
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform target; // Assign your player (character) transform here
        public Vector3 offset = new Vector3(0, 5, -10); // Offset position relative to the target
        public float followSpeed = 5f; // Smooth follow speed

        void LateUpdate()
        {
            if (target != null)
            {
                // Calculate the target position with offset
                Vector3 targetPosition = target.position + offset;

                // Smoothly move the camera towards the target position
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
        }
    }
}