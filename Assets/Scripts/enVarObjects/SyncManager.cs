using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class SyncManager : MonoBehaviour
    {
         public static SyncManager Instance;

        public float forwardSpeed = 2f;
        private float zProgress = 1f;

        public float ZProgress => zProgress;



        private void Awake()
        {
            Instance = this;
           
        }

        private void FixedUpdate()
        {
            zProgress += forwardSpeed * Time.fixedDeltaTime;
        }
    }
}