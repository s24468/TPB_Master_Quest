using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum Biome
{
    T,
    P,
    B
}
[CreateAssetMenu(menuName = "Scriptable Objects/Turret Asset")]
public class TurretAsset : ScriptableObject
{
    [Header("Biome")] public Biome Biome;
    [Header("UISegmentSprites")] 
    public Sprite[] segmentSprites;
    [Header("UIGlow")] public Material material;
    public Color strongColor;
    public Color weakColor;
    public float strongStrength = 3;
}