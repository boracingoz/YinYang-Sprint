using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoulCollector : MonoBehaviour
    {
        [Header("Soul Collection")]
        public int player1CollectedSoul = 0;
        public int player2CollectedSoul = 0;

        public static SoulCollector instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public bool IsBalanced()
        {
            return player1CollectedSoul == player2CollectedSoul;
        }

        public int GetLeadinPLayer()
        {
            if (player1CollectedSoul > player2CollectedSoul)
            {
                return 1;
            }
            else if(player2CollectedSoul > player1CollectedSoul)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }

        public void ResetSouls()
        {
            player1CollectedSoul = 0;
            player2CollectedSoul = 0;
        }
    }
}