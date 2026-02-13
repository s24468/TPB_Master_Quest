using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TurretController :  AttackableBase
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

    private void Update()
    {
        uiTurretController.UpdateHealthVisual(healthFragments);
    }

    public override void ReceiveAttack(string attackerId)
    {
        // 1 hit = -1 fragment
        if (healthFragments <= 0)
        {
            return;
        }

        healthFragments--;
        uiTurretController.UpdateHealthVisual(healthFragments);

        Debug.Log($"Turret {name} hit by {attackerId}. HealthFragments: {healthFragments}");

        if (healthFragments <= 0)
        {
            OnDestroyed(attackerId);
        }
    }

    private void OnDestroyed(string attackerId)
    {
        Debug.Log($"Turret {name} destroyed by {attackerId}");

        // TODO: animacja / efekt / event do logiki gry
        Destroy(gameObject);
    }
}