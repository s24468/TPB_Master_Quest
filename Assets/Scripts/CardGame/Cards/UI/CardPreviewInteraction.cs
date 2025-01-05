using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPreviewInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private bool isHovered = false;
    private Card cardData; // Store this card's data
    [SerializeField] private GameObject hoverPreviewGlow;

    public void Setup(Card data)
    {
        cardData = data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        hoverPreviewGlow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        hoverPreviewGlow.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isHovered)
        {
            CardPreviewManager.Instance.ShowCardPreview(cardData);
        }
    }
}