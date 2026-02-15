using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic 
{
    public Player owner;
    public CardAsset ca;

    public int CasualPower;
    public int TPower;
    public int PPower;
    public int BPower;

    // public CreatureEffect effect;
    public string UniqueCreatureID;

    public int LaneIndex;

    public Table CurrentTable { get; private set; }

    public Biome CurrentBiome => CurrentTable != null ? CurrentTable.Biome : Biome.T; // default jak chcesz


    public bool Frozen = false;

    // STATIC For managing IDs
    public static Dictionary<string, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<string, CreatureLogic>();

    public bool CanAttack
    {
        get
        {
            // bool ownersTurn = (TurnManager.Instance.whoseTurn == owner);
            bool ownersTurn = true;
            return (ownersTurn && (AttacksLeftThisTurn > 0) && !Frozen);
        }
    }

    private int baseAttack;

    // attack with buffs
    public int Attack
    {
        get { return baseAttack; }
    }

    private int attacksForOneTurn = 1;
    public int AttacksLeftThisTurn { get; set; }

    // CONSTRUCTOR new CreatureLogic
    public CreatureLogic(Player owner, CardLogic cardLogic, string uniqueCreatureID, int laneIndex, Table table)
    {
        this.owner = owner;
        this.ca = cardLogic.ca;
        this.LaneIndex = laneIndex;
        this.CurrentTable = table;

        // UniqueCreatureID = cardLogic.UniqueCardID;
        UniqueCreatureID = uniqueCreatureID;
        CasualPower = ca.CasualPower;
        TPower = ca.TPower;
        PPower = ca.PPower;
        BPower = ca.BPower;


        // if (ca.CreatureScriptName!= null && ca.CreatureScriptName!= "")
        // {
        //     effect = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new System.Object[]{owner, this, ca.specialCreatureAmount}) as CreatureEffect;
        //     effect.RegisterEffect();
        // }

        CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
    }

    public void OnTurnStart()
    {
        AttacksLeftThisTurn = attacksForOneTurn;
    }

    public void Die()
    {
        owner.tables[LaneIndex].CreaturesOnTable.Remove(this);
        new CreatureDieCommand(UniqueCreatureID, owner, LaneIndex).AddToQueue();
    }

    public void GoFace()
    {
        AttacksLeftThisTurn--;
        // int targetHealthAfter = owner.otherPlayer.Health - Attack;
        // new CreatureAttackCommand(owner.otherPlayer.PlayerID, UniqueCreatureID, 0, Attack, Health, targetHealthAfter).AddToQueue();
        // owner.otherPlayer.Health -= Attack;
    }

    
    public void AttackCreature(CreatureLogic target)
    {
        AttacksLeftThisTurn--;

        // biome zależy od tego, na jakim stole stoi ATTACKER (czyli Ty)
        Biome biome = CurrentBiome;

        int myPower = GetPowerOnBiome(biome);
        int targetPower = target.GetPowerOnBiome(biome);

        Debug.Log($"Biome={biome} | myPower={myPower} targetPower={targetPower}");

        if (targetPower > myPower)
        {
            Die();
        }
        else if (targetPower < myPower)
        {
            target.Die();
        }
        else
        {
            Debug.Log("Tie!");
        }
    }

    private int GetPowerOnBiome(Biome biome)
    {
        var biomePower = biome switch
        {
            Biome.T => TPower,
            Biome.P => PPower,
            Biome.B => BPower,
            _ => 0
        };
        return CasualPower + biomePower;
    }

    public void AttackCreatureWithID(string uniqueCreatureID)
    {
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target);
    }
}