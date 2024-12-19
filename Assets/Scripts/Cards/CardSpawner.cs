using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab karty
    public Transform cardParent; // Rodzic obiektów kart
    [SerializeField] public Vector3 localCardScale = new Vector3(18f, 18f, 1f); // Rozmiar kart

    private void Awake()
    {
        InstantiateCreatureCards();
    }


    public void InstantiateCreatureCards()
    {
        List<Card> creatureCards = CardDataLoader.Instance.GetCreatureCards();

        if (creatureCards == null || creatureCards.Count == 0)
        {
            Debug.LogWarning("Brak załadowanych kart do stworzenia.");
            return;
        }

        foreach (Card card in creatureCards)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent);
            newCard.transform.localScale = localCardScale;

            // Ustaw dane karty
            CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.Setup(card);
            }
            else
            {
                Debug.LogWarning("Prefab karty nie ma komponentu CardDisplay.");
            }
            // Set up the interaction script
            CardPreviewInteraction cardInteraction = newCard.GetComponent<CardPreviewInteraction>();
            if (cardInteraction != null)
            {
                cardInteraction.Setup(card); // Pass the card data
            }
        }

        Debug.Log("Stworzono " + creatureCards.Count + " kart.");
    }
  
}