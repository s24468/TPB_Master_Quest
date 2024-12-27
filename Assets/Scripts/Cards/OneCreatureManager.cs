using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

public class OneCreatureManager : MonoBehaviour
{
    public CardAsset cardAsset;
    public OneCardManager PreviewManager;
    [Header("Text Component References")] 
    // public Text TPowerText;
    // public Text PPowerText;
    // public Text BPowerText;
    public TextMeshProUGUI TPowerText;
    public TextMeshProUGUI PPowerText;
    public TextMeshProUGUI BPowerText;
    [Header("Image References")] public Image CreatureGraphicImage;
    public Image CreatureGlowImage;

    void Awake()
    {
        if (cardAsset != null)
            ReadCreatureFromAsset();
    }

    private bool canAttackNow = false;

    public bool CanAttackNow
    {
        get { return canAttackNow; }

        set
        {
            canAttackNow = value;

            CreatureGlowImage.enabled = value;
        }
    }

    public void ReadCreatureFromAsset()
    {
        // Change the card graphic sprite
        CreatureGraphicImage.sprite = cardAsset.CardImage;

        // AttackText.text = cardAsset.Attack.ToString();
        // HealthText.text = cardAsset.MaxHealth.ToString();
        // TPowerText.text =  cardAsset.TPower.ToString();
        // PPowerText.text =  cardAsset.PPower.ToString();
        // BPowerText.text =  cardAsset.BPower.ToString();
        TPowerText.SetText(cardAsset.TPower.ToString());
        PPowerText.SetText(cardAsset.PPower.ToString());
        BPowerText.SetText(cardAsset.BPower.ToString());

        if (PreviewManager != null)
        {
            PreviewManager.cardAsset = cardAsset;
            PreviewManager.ReadCardFromAsset();
        }
    }

    public void TakeDamage(int amount, int healthAfter)
    {
        if (amount > 0)
        {
            // TODO DamageEffect.CreateDamageEffect(transform.position, amount);
            // HealthText.text = healthAfter.ToString();
        }
    }
}