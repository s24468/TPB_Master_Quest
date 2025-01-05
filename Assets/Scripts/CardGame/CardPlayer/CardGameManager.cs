using System;
using System.Collections;
using UnityEngine;

namespace Cards
{
    public class CardGameManager : MonoBehaviour
    {
        public GameObject hand;
        public float startNumberOfCards = 5;

        void Start()
        {
            // Rozpocznij korutynę
            StartCoroutine(setHand());
        }

        IEnumerator setHand()
        {
            yield return new WaitForSeconds(1f);
            float duration = startNumberOfCards; // Czas trwania (5 sekund)
            float interval = 1f; // Interwał (1 sekunda)
            float elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                hand.GetComponent<HandVisual>().GivePlayerARandomCard();
                yield return new WaitForSeconds(interval);
                elapsedTime += interval;
            }
        }
    }
}