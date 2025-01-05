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
        public PreviewManager previewManager;

        [Header("Text Component References")] public TextMeshProUGUI nameText;
        public TextMeshProUGUI manaCostText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI tPowerText;
        public TextMeshProUGUI pPowerText;
        public TextMeshProUGUI bPowerText;
        public TextMeshProUGUI casualPowerText;

        [Header("Image References")] public Image cardGraphicImage;

        [FormerlySerializedAs("CardBodyImage")]
        public Image cardBodyImage;

        void Awake()
        {
            if (cardAsset != null)
                ReadCardFromAsset();
        }


        public void ReadCardFromAsset()
        {
            nameText.text = cardAsset.Name;
            manaCostText.text = cardAsset.ManaCost.ToString();
            descriptionText.text = cardAsset.Description;
            cardGraphicImage.sprite = cardAsset.CardImage;

            if (cardAsset.IsCreature)
            {
                tPowerText.SetText(cardAsset.TPower.ToString());
                pPowerText.SetText(cardAsset.PPower.ToString());
                bPowerText.SetText(cardAsset.BPower.ToString());
                casualPowerText.SetText(cardAsset.CasualPower.ToString());
            }

            previewManager.setReferences(cardAsset);
        }
    }
}