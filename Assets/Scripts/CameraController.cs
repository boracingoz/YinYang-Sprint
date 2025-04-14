using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform target;

        [Header("Camera Offset")]
        public Vector3 offset = new Vector3(0, 5, -10);
        public float smoothSpeed = 5f;

        [Header("Look  Settings")]
        public bool lookAtTarget = true;
        public Vector3 lookOfset = new Vector3(0, 1, 0);

        [Header("Rotation Settings")]
        public bool rotateWithTarget = false;

        [Header("Dumping Settings")]
        public bool dampYMovement = true;
        public float verticalSmoothness = 0.5f;


        private float currentYVelocity;
        private Vector3  desiredPosition;
        private Quaternion desiredRotation;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            Vector3 targetOfset;
            if (rotateWithTarget)
            {
                targetOfset = target.TransformDirection(offset); 
            }
            else
            {
                targetOfset = offset;
            }

            desiredPosition = target.position + targetOfset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            if (lookAtTarget)
            {
                transform.LookAt(target.position + lookOfset);
            }

            if (dampYMovement)
            {
                Vector3 currentPos = transform.position;

                float newX = Mathf.Lerp(currentPos.x, desiredPosition.x, smoothSpeed * Time.deltaTime);
                float newZ = Mathf.Lerp(currentPos.z, desiredPosition.z, smoothSpeed * Time.deltaTime);
                float newY = Mathf.SmoothDamp(currentPos.y, desiredPosition.y,
                                ref currentYVelocity, verticalSmoothness);

                transform.position = new Vector3(newX, newY, newZ);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
        }

        private void OnDrawGizmos()
        {
            if (target == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Vector3 targetPos = target.position + offset;
            Gizmos.DrawLine(transform.position, target.position + lookOfset);
            Gizmos.DrawWireSphere(target.position + lookOfset, 0.3f);
        }
    }
}