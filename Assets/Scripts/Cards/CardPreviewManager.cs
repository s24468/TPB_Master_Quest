using DataPersistance;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class CardPreviewManager : MonoBehaviour
{
    public static CardPreviewManager Instance; // Singleton instance

    [Header("UI Components")] public TextMeshProUGUI nameText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI tPowerText;
    public TextMeshProUGUI pPowerText;
    public TextMeshProUGUI bPowerText;
    public TextMeshProUGUI casualPowerText;
    public TextMeshProUGUI descriptionText;
    public Image cardImage; // Image component for the card sprite
    public TextMeshProUGUI costText;
    public TextMeshProUGUI numberOfOwnedCardsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowCardPreview(Card cardData)
    {
        if (cardData == null)
        {
            Debug.LogError("No card data provided for preview.");
            return;
        }

        currentCard = cardData;

        // Update UI elements
        nameText.text = cardData.Name;
        manaText.text = cardData.Mana.ToString();
        cardImage.sprite = cardData.CardSprite;
        casualPowerText.text = cardData.CasualPower.ToString();
        tPowerText.text = "T: " + cardData.TPower.ToString();
        pPowerText.text = "P: " + cardData.PPower.ToString();
        bPowerText.text = "B: " + cardData.BPower.ToString();
        descriptionText.text = cardData.Description;
        costText.text = $"Cost: {cardData.Cost}";

        numberOfOwnedCardsText.text = $"Owned: {cardData.Cost}";

        // Get the number of owned cards using GameData
        string cardId = cardData.ID.ToString(); // Assuming cardData.ID is the card's unique ID
        int ownedCards = DataPersistenceManager.instance.gameData.GetNumberOfOwnedCards(cardId);
        numberOfOwnedCardsText.text = $"Owned: {ownedCards}";
        Debug.Log($"Preview updated for card: {cardData.Name}, Owned: {ownedCards}");
    }

    private Card currentCard; // To keep track of the currently displayed card

    public void BuyCard()
    {
        if (currentCard == null)
        {
            Debug.LogError("No card selected for purchase.");
            return;
        }

        GameData gameData = DataPersistenceManager.instance.gameData;
        string cardId = currentCard.ID.ToString();
        int cardCost = currentCard.Cost;

        if (gameData.money >= cardCost)
        {
            // Deduct money and add the card to the collection
            gameData.money -= cardCost;


            if (gameData.CardDictionaryCollected.ContainsKey(cardId))
            {
                gameData.CardDictionaryCollected[cardId]++;
            }
            else
            {
                gameData.CardDictionaryCollected[cardId] = 1;
            }

            // Update the UI
            numberOfOwnedCardsText.text = $"Owned: {gameData.CardDictionaryCollected[cardId]}";
            Debug.Log($"Bought card: {currentCard.Name}. Remaining money: {gameData.money}");
        }
        else
        {
            Debug.LogWarning("Not enough money to buy this card.");
        }
    }
}