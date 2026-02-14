using UnityEngine;

public class TurretController : AttackableBase
{
    [SerializeField] private UITurretController uiTurretController;
    [SerializeField] private TurretAsset turretAsset;

    [Header("Segments")] [SerializeField] private int maxSegments = 4;
    [SerializeField] private int segmentValue = 250;

    [Header("Runtime (read-only)")] [SerializeField]
    private int segmentsLeft;

    [SerializeField] private int health; // zawsze = segmentsLeft * segmentValue

    private Biome biome;
    public Biome Biome => biome;

    private void Awake()
    {
        biome = turretAsset.Biome;

        // startowe wartości
        segmentsLeft = maxSegments;
        health = segmentsLeft * segmentValue;

        InitializeUI();
    }

    private void InitializeUI()
    {
        uiTurretController.InitializeUISegments(
            turretAsset.segmentSprites,
            turretAsset.material,
            turretAsset.strongColor,
            turretAsset.weakColor
        );

        uiTurretController.UpdateHealthVisual(segmentsLeft);
        uiTurretController.SetLetter(biome.ToString());
    }

    public override void ReceiveAttack(string attackerId)
    {
        if (segmentsLeft <= 0) return;

        if (string.IsNullOrEmpty(attackerId) ||
            !CreatureLogic.CreaturesCreatedThisGame.TryGetValue(attackerId, out var attacker) ||
            attacker == null)
        {
            Debug.LogWarning($"Turret {name}: attackerId not found/invalid: {attackerId}");
            return;
        }

        int damage = ComputeDamageFrom(attacker);

        // ile segmentów ma spaść?
        int segmentsLost = ComputeSegmentsLost(damage);

        segmentsLeft = Mathf.Max(0, segmentsLeft - segmentsLost);

        // "snap" health do liczby segmentów
        health = segmentsLeft * segmentValue;

        uiTurretController.UpdateHealthVisual(segmentsLeft);

        Debug.Log(
            $"Turret {name} hit by {attackerId}. Biome={biome}, Damage={damage}, SegmentsLost={segmentsLost}, SegmentsLeft={segmentsLeft}, Health={health}");

        if (segmentsLeft <= 0)
        {
            OnDestroyed(attackerId);
        }
    }

    private int ComputeDamageFrom(CreatureLogic attacker)
    {
        int biomePower = biome switch
        {
            Biome.T => attacker.TPower,
            Biome.P => attacker.PPower,
            Biome.B => attacker.BPower,
            _ => 0
        };

        return attacker.CasualPower + biomePower;
    }

    private int ComputeSegmentsLost(int damage)
    {
        if (damage <= 0) return 0;
        // 250*2 < 600 < 250*3 => floor(600/250)=2 segmenty
        int lost = damage / segmentValue;
        return Mathf.Min(lost, segmentsLeft);
    }

    private void OnDestroyed(string attackerId)
    {
        Debug.Log($"Turret {name} destroyed by {attackerId}");
        Destroy(gameObject);
    }
}