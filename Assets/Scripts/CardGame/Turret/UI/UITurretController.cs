using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum SegmentPosition
{
    TopRight,
    DownRight,
    DownLeft,
    TopLeft
}
//segments[(int)SegmentPosition.DownRight].SetStrong()

public class UITurretController : MonoBehaviour
{
    [SerializeField] public UISegment[] segments;
    [SerializeField] public TextMeshProUGUI letter;

    public void SetLetter(string let)
    {
        letter.text = let;
    }

    public void InitializeUISegments(Sprite[] sprites, Material material, Color strongColor, float strongStrength,
        Color weakColor)
    {
        for (var i = 0; i < sprites.Length; i++)
        {
            segments[i].SetSprite(sprites[i]);
            segments[i].strongStrength = strongStrength;
            segments[i].strongColor = strongColor;
            segments[i].weakColor = weakColor;
            segments[i].SetMaterial(material);
        }
    }

    public void UpdateHealthVisual(int healthFragments)
    {
        int clamped = Mathf.Clamp(healthFragments, 0, segments.Length);

        for (int i = 0; i < segments.Length; i++)
        {
            if (i < clamped)
            {
                segments[i].SetStrong();
            }
            else
            {
                segments[i].SetWeak();
            }
        }
    }
}