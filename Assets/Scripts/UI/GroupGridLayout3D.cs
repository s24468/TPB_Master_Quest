using UnityEngine;
using System.Collections;

// place first and last elements in children array manually
// others will be placed automatically with equal distances between first and last elements
public class GroupGridLayout3D : MonoBehaviour {
	
    [Header("Grid Settings")]
    public Vector3 spacing = new Vector3(32, 50, 0); // Spacing between objects
    public int columns = 3; // Number of columns
    public bool centerGrid = false; // Optional centering

    [Header("Offset Settings")]
    public Vector3 startOffset = new Vector3(0, -50, 0); // Offset to adjust starting position

    [Header("Auto Adjust Content Size")]
    public bool autoAdjustContentSize = true; // Dynamically adjust content size

    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        ArrangeObjects();
    }

    public void ArrangeObjects()
    {
        int childCount = transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);

            // Calculate grid position
            int row = i / columns;
            int column = i % columns;

            Vector3 position = new Vector3(
                column * spacing.x + startOffset.x,  // Apply X offset
                -row * spacing.y + startOffset.y,   // Apply Y offset
                startOffset.z                       // Apply Z offset
            );

            child.localPosition = position;
        }

        // Adjust content size for ScrollRect
        if (autoAdjustContentSize && _rectTransform != null)
        {
            int totalRows = Mathf.CeilToInt((float)childCount / columns);
            Vector2 size = new Vector2(
                columns * spacing.x,
                totalRows * spacing.y
            );

            _rectTransform.sizeDelta = size;
        }
    }
}
