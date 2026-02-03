using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cards
{
    public class OneCreatureManager : MonoBehaviour
    {
        public CardAsset cardAsset;
        public PreviewManager previewManager;
        [Header("Text Component References")] public TextMeshProUGUI nameText;
        public TextMeshProUGUI manaCostText;
        public TextMeshProUGUI casualpowerText;
        public TextMeshProUGUI tpowerText;
        public TextMeshProUGUI pPowerText;
        public TextMeshProUGUI bPowerText;
        [Header("Image References")] public Image creatureGraphicImage;
        public Image creatureGlowImage;

        void Awake()
        {
            if (cardAsset != null)
                ReadCreatureFromAsset();
        }

        private bool _canAttackNow = true;

        public bool CanAttackNow
        {
            get => _canAttackNow;

            set
            {
                _canAttackNow = value;

                creatureGlowImage.enabled = value;
            }
        }

        public void ReadCreatureFromAsset()
        {
            // Change the card graphic sprite
            creatureGraphicImage.sprite = cardAsset.CardImage;
            nameText.text = cardAsset.Name;
            manaCostText.text = cardAsset.ManaCost.ToString();
            casualpowerText.SetText(cardAsset.CasualPower.ToString());
            tpowerText.SetText(cardAsset.TPower.ToString());
            pPowerText.SetText(cardAsset.PPower.ToString());
            bPowerText.SetText(cardAsset.BPower.ToString());

            previewManager.setReferences(cardAsset);
            // previewManager.cardAsset = cardAsset;
            // previewManager.ReadCardFromAsset();
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
}