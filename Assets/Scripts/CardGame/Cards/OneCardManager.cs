using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// holds the refs to all the Text, Images on the card
namespace Cards
{
    public class OneCardManager : MonoBehaviour
    {
        public CardAsset cardAsset;

        // public OneCardManager previewManager;

        // [Header("Text Component References")] public Text NameText;
        public TextMeshProUGUI nameText;

        // public Text ManaCostText;
        public TextMeshProUGUI manaCostText;

        // public Text DescriptionText;
        public TextMeshProUGUI descriptionText;

        public TextMeshProUGUI tPowerText;
        public TextMeshProUGUI pPowerText;
        public TextMeshProUGUI bPowerText;

        [Header("Image References")]
        // public Image CardTopRibbonImage;
        // public Image CardLowRibbonImage;
        public Image cardGraphicImage;

        [FormerlySerializedAs("CardBodyImage")]
        public Image cardBodyImage;

        [FormerlySerializedAs("CardFaceFrameImage")]
        public Image cardFaceFrameImage;

        [FormerlySerializedAs("CardFaceGlowImage")]
        public Image cardFaceGlowImage;

        [FormerlySerializedAs("CardBackGlowImage")]
        public Image cardBackGlowImage;

        void Awake()
        {
            if (cardAsset != null)
                ReadCardFromAsset();
        }

        private bool _canBePlayedNow = false;

        public bool CanBePlayedNow
        {
            get => _canBePlayedNow;

            set
            {
                _canBePlayedNow = value;

                cardFaceGlowImage.enabled = value;
            }
        }

        public void ReadCardFromAsset()
        {
            // universal actions for any Card
            // 1) apply tint
            // if (cardAsset.characterAsset != null)
            // {
            //     CardBodyImage.color = cardAsset.characterAsset.ClassCardTint;
            //     CardFaceFrameImage.color = cardAsset.characterAsset.ClassCardTint;
            //     // CardTopRibbonImage.color = cardAsset.characterAsset.ClassRibbonsTint;
            //     // CardLowRibbonImage.color = cardAsset.characterAsset.ClassRibbonsTint;
            // }
            // else
            // {
            //     // CardBodyImage.color = GlobalSettings.Instance.CardBodyStandardColor;
            //     CardFaceFrameImage.color = Color.white;
            //     // CardTopRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            //     // CardLowRibbonImage.color = GlobalSettings.Instance.CardRibbonsStandardColor;
            // }

            // 2) add card name
            nameText.text = cardAsset.name;
            // 3) add mana cost
            manaCostText.text = cardAsset.ManaCost.ToString();
            // 4) add description
            descriptionText.text = cardAsset.Description;
            // 5) Change the card graphic sprite
            cardGraphicImage.sprite = cardAsset.CardImage;

            if (cardAsset.IsCreature)
            {
            //     // this is a creature
            //     // AttackText.text = cardAsset.Attack.ToString();
            //     // HealthText.text = cardAsset.MaxHealth.ToString();
            //     tPowerText = gameObject.AddComponent<TextMeshProUGUI>();
            //     pPowerText = gameObject.AddComponent<TextMeshProUGUI>();
            //     bPowerText = gameObject.AddComponent<TextMeshProUGUI>();
            tPowerText.SetText(cardAsset.TPower.ToString());
            pPowerText.SetText(cardAsset.PPower.ToString());
            bPowerText.SetText(cardAsset.BPower.ToString());
            }
            

            // if (previewManager != null)
            // {
            //     // this is a card and not a preview
            //     //  GameObject will have OneCardManager as well, but PreviewManager should be null there
            //     previewManager.cardAsset = cardAsset;
            //     previewManager.ReadCardFromAsset();
            // }
        }
    }
}