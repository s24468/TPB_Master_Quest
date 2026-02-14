using UnityEngine;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();

    [Header("Lane / Biome")]
    [SerializeField] private TurretController linkedTurret;
    [SerializeField] private Biome biomeOverride;
    [SerializeField] private bool useTurretBiome = true;

    public Biome Biome
    {
        get
        {
            if (useTurretBiome && linkedTurret != null)
                return linkedTurret.Biome;

            return biomeOverride;
        }
    }

    public void PlaceCreatureAt(int index, CreatureLogic creature)
    {
        CreaturesOnTable.Insert(index, creature);
    }
}
