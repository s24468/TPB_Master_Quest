using UnityEngine;
using UnityEngine.EventSystems;

public class CardHoverPreviewToTop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform previewTransform; // np. CreatureCard/Canvas/CardPanel/Preview
    [SerializeField] private RectTransform hoverRoot;        // obiekt na scenie "nad wszystkim"

    [Header("Preview placement")]
    [SerializeField] private Vector2 hoverAnchoredPos = new Vector2(0f, 0f); // np. środek
    [SerializeField] private Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);

    private Transform originalParent;
    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private Vector3 originalLocalScale;
    private int originalSiblingIndex;
    void Awake()
    {
        if (hoverRoot == null)
            hoverRoot = GameObject.Find("HoverRoot")?.GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (previewTransform == null || hoverRoot == null) return;

        originalParent = previewTransform.parent;
        originalSiblingIndex = previewTransform.GetSiblingIndex();
        originalLocalPos = previewTransform.localPosition;
        originalLocalRot = previewTransform.localRotation;
        originalLocalScale = previewTransform.localScale;

        // przerzuć preview na wierzch
        previewTransform.SetParent(hoverRoot, worldPositionStays: false);
        previewTransform.SetAsLastSibling();

        // ustaw gdzie ma być (anchoredPosition działa, jeśli to UI/RectTransform)
        previewTransform.anchoredPosition = hoverAnchoredPos;
        previewTransform.localRotation = Quaternion.identity;
        previewTransform.localScale = hoverScale;

        previewTransform.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (previewTransform == null) return;

        // wróć do oryginalnego parenta i ustawień
        previewTransform.SetParent(originalParent, worldPositionStays: false);
        previewTransform.SetSiblingIndex(originalSiblingIndex);

        previewTransform.localPosition = originalLocalPos;
        previewTransform.localRotation = originalLocalRot;
        previewTransform.localScale = originalLocalScale;
    }
}
