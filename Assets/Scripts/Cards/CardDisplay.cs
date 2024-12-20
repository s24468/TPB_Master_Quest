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
        // if (tPowerText != null)
        //     if (cardData.TPower==-1)
        //     {
        //         tPowerText.text = "";
        //     }
        //     tPowerText.text = "T: " + cardData.TPower.ToString();
        // if (pPowerText != null)
        //     pPowerText.text = "P: " + cardData.PPower.ToString();
        // if (bPowerText != null)
        //     bPowerText.text = "B: " + cardData.BPower.ToString();
        // if (casualPowerText != null)
        //     casualPowerText.text = cardData.CasualPower.ToString();
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

        // Set the card image
        if (cardImage != null && cardData.CardSprite != null)
        {
            cardImage.sprite = cardData.CardSprite;
        }
    }
}