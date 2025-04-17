using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform target;

        [Header("Camera Settings")]
        public CameraSettings cameraSettings;


        private float currentYVelocity;
        private Vector3  desiredPosition;

        private void LateUpdate()
        {
            if (target == null || cameraSettings == null)
            {
                return;
            }

            Vector3 targetOfset;
            if (cameraSettings.rotateWithTarget)
            {
                targetOfset = target.TransformDirection(cameraSettings.offset); 
            }
            else
            {
                targetOfset = cameraSettings.offset;
            }

            desiredPosition = target.position + targetOfset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSettings.smoothSpeed * Time.deltaTime);

            if (cameraSettings.lookAtTarget)
            {
                transform.LookAt(target.position + cameraSettings.lookOffset);
            }

            if (cameraSettings.dampYMovement)
            {
                Vector3 currentPos = transform.position;

                float newX = Mathf.Lerp(currentPos.x, desiredPosition.x, cameraSettings.smoothSpeed * Time.deltaTime);
                float newZ = Mathf.Lerp(currentPos.z, desiredPosition.z, cameraSettings.smoothSpeed * Time.deltaTime);
                float newY = Mathf.SmoothDamp(currentPos.y, desiredPosition.y,
                                ref currentYVelocity, cameraSettings.verticalSmoothness);

                transform.position = new Vector3(newX, newY, newZ);
            }
            else
            {
                transform.position = Vector3.Lerp(transform.position, desiredPosition, cameraSettings.smoothSpeed * Time.deltaTime);
            }
        }

        private void OnDrawGizmos()
        {
            if (target == null)
            {
                return;
            }

            Gizmos.color = Color.yellow;
            Vector3 targetPos = target.position + cameraSettings.offset;
            Gizmos.DrawLine(transform.position, target.position + cameraSettings.lookOffset);
            Gizmos.DrawWireSphere(target.position + cameraSettings.lookOffset, 0.3f);
        }
    }
}