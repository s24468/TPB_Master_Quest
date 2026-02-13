using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretController : MonoBehaviour
{
    [SerializeField] private UITurretController uiTurretController;
    [SerializeField] private TurretAsset turretAsset;
    [SerializeField] private UISegment[] uiSegmentGlow;
    [SerializeField] private int healthFragments = 4;
    private Biome biome;

    private void Awake()
    {
        biome = turretAsset.Biome;
        InitializeUI();
    }

    private void InitializeUI()
    {
        uiTurretController.InitializeUISegments(turretAsset.segmentSprites, turretAsset.material,
            turretAsset.strongColor,
            turretAsset.weakColor);
        uiTurretController.UpdateHealthVisual(healthFragments);
        uiTurretController.SetLetter(biome.ToString());
    }
}