using UnityEngine;
using System.Collections;

// place first and last elements in children array manually
// others will be placed automatically with equal distances between first and last elements
public class GroupGridLayout3D : MonoBehaviour {
	
	[Header("Grid Settings")]
	public Vector3 spacing = new Vector3(2f, 2f, 2f); // Spacing between objects
	public int columns = 3; // Number of columns
	public int rows = 3;    // Number of rows per layer
	public bool autoArrangeOnStart = true; // Automatically arrange on start

	[Header("Alignment Options")]
	public bool centerGrid = true; // Center the grid around the parent position

	void Start()
	{
		if (autoArrangeOnStart)
			ArrangeObjects();
	}

	public void ArrangeObjects()
	{
		int childCount = transform.childCount;

		for (int i = 0; i < childCount; i++)
		{
			Transform child = transform.GetChild(i);

			// Calculate grid position
			int layer = i / (columns * rows); // Depth (Z-axis layer)
			int row = (i % (columns * rows)) / columns; // Row index
			int column = i % columns; // Column index

			Vector3 position = new Vector3(
				column * spacing.x,
				-row * spacing.y,
				-layer * spacing.z
			);

			// Optional: Center grid around the parent position
			if (centerGrid)
			{
				Vector3 gridCenterOffset = new Vector3(
					(columns - 1) * spacing.x * -0.5f,
					(rows - 1) * spacing.y * 0.5f,
					layer * spacing.z * 0.5f
				);
				position += gridCenterOffset;
			}

			// Set child's position relative to parent
			child.localPosition = position;
		}
	}
}
