using Assets.Scripts.Managers;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class Soul : MonoBehaviour
    {
        [Header("Soult Settings")]
        public float moveSpeed = 2f;
        public float despawnDistance = -10f;


        private void Start()
        {
            Collider collider = GetComponent<Collider>();
            if (collider != null && !collider.isTrigger)
            {
                collider.isTrigger = true;
            }

            if (GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = gameObject.AddComponent<Rigidbody>();
                rb.useGravity = false;
                rb.isKinematic = true;
            }
        }

        void Update()
        {
            transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);

            if (transform.position.z < despawnDistance)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Soul hit: " + other.gameObject);

            CustomCharacterController controller = other.GetComponent<CustomCharacterController>();

            if (controller != null)
            {
                if (!controller.userArrowKeys)
                {
                    SoulCollector.instance.player1CollectedSoul++;
                    Debug.Log("Player 1 collected a soul! total: " + SoulCollector.instance.player1CollectedSoul);
                }
                else
                {
                    SoulCollector.instance.player2CollectedSoul++;
                    Debug.Log("Player 2 collected a soul! total: " + SoulCollector.instance.player2CollectedSoul);
                }

                if (DifficultyManager.instance != null)
                {
                    DifficultyManager.instance.UpdateDifficulty();
                }

                Destroy(gameObject);
            }


        }
    }
}