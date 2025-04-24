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
            }

            DifficultyManager.instance.UpdateDifficulty();

            Destroy(gameObject);
        }
    }
}