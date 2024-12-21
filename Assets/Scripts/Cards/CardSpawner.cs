using UnityEngine;
using System.Collections.Generic;
using UI;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab karty

    // public Transform cardParent; // Rodzic obiektów kart
    public Transform creatureCardParent; // Rodzic obiektów kart
    public Transform spellCardParent; // Rodzic obiektów kart
    [SerializeField] public Vector3 localCardScale = new Vector3(18f, 18f, 1f); // Rozmiar kart

    private List<GameObject> cardPool = new List<GameObject>();

    private List<GameObject> creatureCardPool = new List<GameObject>();
    private List<GameObject> spellCardPool = new List<GameObject>();
    // private int poolSize = 52; // Adjust this based on your requirements


    public void InitializePools()
    {
        InitializePool(creatureCardPool, CardDataLoader.Instance.GetCreatureCards(), creatureCardParent);
        InitializePool(spellCardPool, CardDataLoader.Instance.GetSpellCards(), spellCardParent);
    }

    private void InitializePool(List<GameObject> pool, List<Card> cards, Transform parent)
    {
        foreach (Card card in cards)
        {
            GameObject newCard = Instantiate(cardPrefab, parent);
            newCard.SetActive(false); // Deactivate by default
            newCard.transform.localScale = localCardScale; // Ensure correct scale
            pool.Add(newCard);
        }
    }

    public void DisplayCreatureCards()
    {
        DisplayCards(creatureCardPool, CardDataLoader.Instance.GetCreatureCards());
    }

    public void DisplaySpellCards()
    {
        DisplayCards(spellCardPool, CardDataLoader.Instance.GetSpellCards());
    }

    private void DisplayCards(List<GameObject> pool, List<Card> cards)
    {
        // Deactivate all cards in the pool
        foreach (GameObject card in spellCardPool)
        {
            card.SetActive(false);
        }

        foreach (GameObject card in creatureCardPool)
        {
            card.SetActive(false);
        }

        // Activate and update cards based on the input list
        for (int i = 0; i < cards.Count; i++)
        {
            if (i >= pool.Count)
            {
                Debug.LogError("Not enough cards in the pool to display!");
                return;
            }

            GameObject card = pool[i];
            card.SetActive(true);

            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
            if (cardDisplay != null)
            {
                cardDisplay.Setup(cards[i]);
            }

            CardPreviewInteraction cardInteraction = card.GetComponent<CardPreviewInteraction>();
            if (cardInteraction != null)
            {
                cardInteraction.Setup(cards[i]);
            }
        }
    }
    
    
}
// public void InitializePool()
    // {
    //     List<Card> allCards = new List<Card>();
    //     // List<Card> cards = CardDataLoader.Instance.GetCreatureCards();
    //     allCards.AddRange(CardDataLoader.Instance.GetCreatureCards());
    //     allCards.AddRange(CardDataLoader.Instance.GetSpellCards());
    //     poolSize = allCards.Count;
    //     for (int i = 0; i < poolSize; i++)
    //     {
    //         GameObject newCard = Instantiate(cardPrefab, cardParent);
    //         newCard.SetActive(false); // Deactivate by default
    //         newCard.transform.localScale = localCardScale; // Ensure correct scale
    //         cardPool.Add(newCard);
    //     }
    // }
    // public void DisplayCreatureCards()
    // {
    //     // ClearCards();
    //     // InstantiateCards(CardDataLoader.Instance.GetCreatureCards());
    //     // int y = 0;
    //     // foreach (Card x in CardDataLoader.Instance.GetSpellCards())
    //     // {
    //     //     print(CardDataLoader.Instance.GetSpellCards().Count);
    //     //     print(CardDataLoader.Instance.GetCreatureCards().Count);
    //     // }
    //
    //     DisplayCards(CardDataLoader.Instance.GetCreatureCards());
    // }
    //
    // public void DisplaySpellCards()
    // {
    //     // ClearCards();
    //     // InstantiateCards(CardDataLoader.Instance.GetSpellCards());
    //
    //     DisplayCards(CardDataLoader.Instance.GetSpellCards(), CardDataLoader.Instance.GetCreatureCards().Count - 1);
    // }

    // public void DisplayCards(List<Card> cards, int firstPlaceInCards = 0)
    // {
    //     // Deactivate all cards in the pool
    //     foreach (GameObject card in cardPool)
    //     {
    //         card.SetActive(false);
    //     }
    //
    //     // Activate and update cards based on the input list
    //     // for (int i = firstPlaceInCards; i < firstPlaceInCards + cards.Count; i++)
    //     // {
    //     //     Debug.Log(i + "X" + cardPool[i] + " " + cards.Count + " " + firstPlaceInCards);
    //     //     GameObject card = cardPool[i];
    //     //     card.SetActive(true);
    //     //
    //     //     CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
    //     //     if (cardDisplay != null)
    //     //     {
    //     //         cardDisplay.Setup(cards[i - firstPlaceInCards]);
    //     //     }
    //     //
    //     //     CardPreviewInteraction cardInteraction = card.GetComponent<CardPreviewInteraction>();
    //     //     if (cardInteraction != null)
    //     //     {
    //     //         cardInteraction.Setup(cards[i - firstPlaceInCards]); // Pass the card data
    //     //     }
    //     // }
    //     for (int i = 0; i < cards.Count; i++)
    //     {
    //         GameObject card = cardPool[firstPlaceInCards + i];
    //         card.SetActive(true);
    //
    //         CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
    //         cardDisplay?.Setup(cards[i]);
    //
    //         CardPreviewInteraction cardInteraction = card.GetComponent<CardPreviewInteraction>();
    //         cardInteraction?.Setup(cards[i]);
    //     }
    // }

    // public void InstantiateCards(List<Card> cards)
    // {
    //     if (cards == null || cards.Count == 0)
    //     {
    //         Debug.LogWarning("Brak kart do wyświetlenia.");
    //         return;
    //     }
    //
    //     // List<Card> creatureCards = CardDataLoader.Instance.GetCreatureCards();
    //     //
    //     // if (creatureCards == null || creatureCards.Count == 0)
    //     // {
    //     //     Debug.LogWarning("Brak załadowanych kart do stworzenia.");
    //     //     return;
    //     // }
    //
    //     foreach (Card card in cards)
    //     {
    //         GameObject newCard = Instantiate(cardPrefab, cardParent);
    //         newCard.transform.localScale = localCardScale;
    //
    //         // Ustaw dane karty
    //         CardDisplay cardDisplay = newCard.GetComponent<CardDisplay>();
    //         if (cardDisplay != null)
    //         {
    //             cardDisplay.Setup(card);
    //         }
    //         else
    //         {
    //             Debug.LogWarning("Prefab karty nie ma komponentu CardDisplay.");
    //         }
    //
    //         // Set up the interaction script
    //         CardPreviewInteraction cardInteraction = newCard.GetComponent<CardPreviewInteraction>();
    //         if (cardInteraction != null)
    //         {
    //             cardInteraction.Setup(card); // Pass the card data
    //         }
    //     }
    //
    //     Debug.Log("Stworzono " + cards.Count + " kart.");
    // }

    // private void ClearCards()
    // {
    //     foreach (Transform child in cardParent)
    //     {
    //         Destroy(child.gameObject);
    //     }
    // }