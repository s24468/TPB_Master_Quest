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