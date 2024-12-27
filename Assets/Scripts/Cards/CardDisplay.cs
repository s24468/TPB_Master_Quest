using UnityEngine;
using TMPro;
using UI;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("UI Components")] public TextMeshProUGUI nameText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI tPowerText;
    public TextMeshProUGUI pPowerText;
    public TextMeshProUGUI bPowerText;
    public TextMeshProUGUI casualPowerText;
    public TextMeshProUGUI descriptionText;
    public Image cardImage; // Image component for the card sprite
    private Card cardData;
    public GameObject NotUnlocked; // Reference to the "NotUnlocked" GameObject

    public void Setup(Card card)
    {
        cardData = card;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        UpdateNotUnlocked();
        if (nameText != null)
            nameText.text = cardData.Name;
        if (manaText != null)
            manaText.text = cardData.Mana.ToString();
        if (tPowerText != null)
        {
            tPowerText.text = cardData.TPower == -1 ? "" : "T: " + cardData.TPower.ToString();
        }

        if (pPowerText != null)
        {
            pPowerText.text = cardData.PPower == -1 ? "" : "P: " + cardData.PPower.ToString();
        }

        if (bPowerText != null)
        {
            bPowerText.text = cardData.BPower == -1 ? "" : "B: " + cardData.BPower.ToString();
        }

        if (casualPowerText != null)
        {
            casualPowerText.text = cardData.CasualPower == -1 ? "" : cardData.CasualPower.ToString();
        }

        if (descriptionText != null)
            descriptionText.text = "• " + cardData.Description;
        if (cardImage != null && cardData.CardSprite != null)
        {
            cardImage.sprite = cardData.CardSprite;
        }
    }

    public void UpdateNotUnlocked()
    {
        if (cardData == null)
        {
            Debug.LogWarning("Card data is null in CardDisplay! Skipping UpdateNotUnlocked.");
            return;
        }

        var cardKey = cardData.ID.ToString();
        var isUnlocked = DataPersistenceManager.instance.gameData.CardDictionaryCollected.ContainsKey(cardKey);
        NotUnlocked.SetActive(!isUnlocked);
    }
}