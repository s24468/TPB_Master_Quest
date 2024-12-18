// using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using TMPro;
using UI;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI tPowerText;
    public TextMeshProUGUI pPowerText;
    public TextMeshProUGUI bPowerText;
    public TextMeshProUGUI casualPowerText;
    public TextMeshProUGUI descriptionText;
    public Image cardImage; // Image component for the card sprite
    private Card cardData;

    public void Setup(Card card)
    {
        cardData = card;
        UpdateDisplay();
    }

    void UpdateDisplay()
    {
        if (nameText != null)
            nameText.text = cardData.Name;
        if (manaText != null)
            manaText.text = cardData.Mana.ToString();
        if (tPowerText != null)
            tPowerText.text = "T: "+cardData.TPower.ToString();
        if (pPowerText != null)
            pPowerText.text = "P: "+cardData.PPower.ToString();
        if (bPowerText != null)
            bPowerText.text = "B: "+cardData.BPower.ToString();
        if (casualPowerText != null)
            casualPowerText.text = cardData.CasualPower.ToString();
        if (descriptionText != null)
            descriptionText.text = "• "+cardData.Description;
        
        // Set the card image
        if (cardImage != null && cardData.CardSprite != null)
        {
            cardImage.sprite = cardData.CardSprite;
        }
    }
}