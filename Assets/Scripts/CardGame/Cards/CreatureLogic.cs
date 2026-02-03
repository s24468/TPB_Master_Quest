using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic //: MonoBehaviour//ICharacter 
{
    // PUBLIC FIELDS
    public Player owner;
    public CardAsset ca;

    // [Header("Creature Info")]
    public int CasualPower;
    public int TPower;
    public int PPower;
    public int BPower;

    // public CreatureEffect effect;
    public string UniqueCreatureID;

    public string ID
    {
        get { return UniqueCreatureID; }
    }

    public bool Frozen = false;

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

    // CONSTRUCTOR
    public CreatureLogic(Player owner, CardAsset ca)
    {
        this.ca = ca;
        // baseHealth = ca.MaxHealth;
        // Health = ca.MaxHealth;
        // baseAttack = ca.Attack;
        // attacksForOneTurn = ca.AttacksForOneTurn;
        // // AttacksLeftThisTurn is now equal to 0
        // if (ca.Charge)
        //     AttacksLeftThisTurn = attacksForOneTurn;
        this.owner = owner;

        // UniqueCreatureID = IDFactory.GetUniqueID();
        UniqueCreatureID = ca.Id;

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
        owner.table.CreaturesOnTable.Remove(this); // usuwa z skryptu Table
        new CreatureDieCommand(UniqueCreatureID, owner).AddToQueue();
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
        Debug.Log("target's casual power: " + target.CasualPower + " , target's Tpower: " + target.TPower);
        Debug.Log("Creature's casual power: " + CasualPower + " , creature's Tpower: " + TPower);
        if (target.CasualPower + target.TPower > CasualPower + TPower)
        {
            Die();
        }
        else if (target.CasualPower + target.TPower < CasualPower + TPower)
        {
            target.Die();
        }
        else
        {
            Debug.Log("Tie!");
        }
    }

    public void AttackCreatureWithID(string uniqueCreatureID)
    {
        CreatureLogic target = CreatureLogic.CreaturesCreatedThisGame[uniqueCreatureID];
        AttackCreature(target);
    }

    // STATIC For managing IDs
    public static Dictionary<string, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<string, CreatureLogic>();

    // the basic health that we have in CardAsset
    private int baseHealth;

    // health with all the current buffs taken into account
    public int MaxHealth
    {
        get { return baseHealth; }
    }

    private int health;

    public int Health
    {
        get { return health; }

        set
        {
            if (value > MaxHealth)
                health = baseHealth;
            else if (value <= 0)
            {
                // Die();
                Debug.Log("haetyhsrathbsrgtfhnbhfghjd");
            }
            else
                health = value;
        }
    }
}