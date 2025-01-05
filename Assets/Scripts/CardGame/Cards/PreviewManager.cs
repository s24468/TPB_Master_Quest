using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cards
{
    public class PreviewManager : MonoBehaviour
    {

        [Header("Text Component References")] public TextMeshProUGUI nameTextPreview;
        public TextMeshProUGUI manaCostTextPreview;
        public TextMeshProUGUI descriptionTextPreview;
        public TextMeshProUGUI tPowerTextPreview;
        public TextMeshProUGUI pPowerTextPreview;
        public TextMeshProUGUI bPowerTextPreview;
        public TextMeshProUGUI casualPowerTextPreview;

        [Header("Image References")] public Image cardGraphicImagePreview;

        [FormerlySerializedAs("CardBodyImage")]
        public Image cardBodyImage;

        public void setReferences(CardAsset cardAsset)
        {
            nameTextPreview.text = cardAsset.Name;
            manaCostTextPreview.text = cardAsset.ManaCost.ToString();
            descriptionTextPreview.text = cardAsset.Description;
            cardGraphicImagePreview.sprite = cardAsset.CardImage;

            if (cardAsset.IsCreature)
            {

                tPowerTextPreview.SetText(cardAsset.TPower.ToString());
                pPowerTextPreview.SetText(cardAsset.PPower.ToString());
                bPowerTextPreview.SetText(cardAsset.BPower.ToString());
                casualPowerTextPreview.SetText(cardAsset.CasualPower.ToString());
            }
        }
    }
}