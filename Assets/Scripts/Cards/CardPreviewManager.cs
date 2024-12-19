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
        Debug.Log($"Preview updated for card: {cardData.Name}");
    }
}