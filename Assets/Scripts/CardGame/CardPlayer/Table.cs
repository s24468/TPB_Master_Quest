// using UnityEngine;
// using System.Collections;
// using System.Collections.Generic;
//
// public class Table : MonoBehaviour 
// {
//     // public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();
//     public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();
//
//     public void PlaceCreatureAt(int index, CreatureLogic creature)
//     {
//         CreaturesOnTable.Insert(index, creature);
//     }
//         
// }
using UnityEngine;
using System.Collections.Generic;

public class Table : MonoBehaviour
{
    public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();

    [Header("Lane / Biome")]
    [SerializeField] private TurretController linkedTurret;   // podłączysz w Inspectorze
    [SerializeField] private Biome biomeOverride;             // opcjonalnie, jak chcesz ręcznie
    [SerializeField] private bool useTurretBiome = true;

    public Biome Biome
    {
        get
        {
            if (useTurretBiome && linkedTurret != null)
                return linkedTurret.Biome; // dodamy public getter w TurretController

            return biomeOverride;
        }
    }

    public void PlaceCreatureAt(int index, CreatureLogic creature)
    {
        CreaturesOnTable.Insert(index, creature);
    }
}
