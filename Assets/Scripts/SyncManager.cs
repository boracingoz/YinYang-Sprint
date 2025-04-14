using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class SyncManager : MonoBehaviour
    {
        public CharacterController[] characters;
        public float forwardSpeed = 5f;

        private float zProgress = 0f;

        private void Start()
        {
            foreach (var character in characters)
            {
                character.forwardSpeed = forwardSpeed;
            }
        }

        private void FixedUpdate()
        {
            zProgress += forwardSpeed * Time.fixedDeltaTime;

            foreach (var character in characters)
            {
                character.UpdateZpos(zProgress);
            }
        }
    }
}