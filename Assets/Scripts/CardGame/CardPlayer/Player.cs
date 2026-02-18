using UnityEngine;
using System.Collections;
using Cards;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public int PlayerID;
    public Color color = Color.white;
    public PlayerArea PArea;
    public Deck deck;
    public HandManager hand;
    public Table[] tables = new Table[3];

    public int ManaLeft
    {
        get { return PArea.ManaPool.logic.AvailableCrystals; }
    }

    public Player otherPlayer
    {
        get { return Players[0] == this ? Players[1] : Players[0]; }
    }

    public bool SacrificeUsedThisTurn;

    public delegate void VoidWithNoArguments();

    public event VoidWithNoArguments EndTurnEvent;

    public static Player[] Players;


    void Awake()
    {
        Players = GameObject.FindObjectsOfType<Player>();
    }

    public void DrawACard()
    {
        new DrawACardCommand(this).AddToQueue();
    }


    public void PlayACreatureFromHand(string UniqueID, int laneIndex, int tablePos)
    {
        var playedCard = CardLogic.CardsCreatedThisGame[UniqueID];
        if (ManaLeft < playedCard.CurrentManaCost)
        {
            Debug.LogWarning(
                $"[MANA] Not enough mana to play {playedCard.ca.name}. Needed: {playedCard.CurrentManaCost}, have: {ManaLeft}");
            return;
        }

        new SpendManaCommand(this, playedCard.CurrentManaCost).AddToQueue();
        new PlayACreatureCommand(playedCard, this, laneIndex, tablePos).AddToQueue();
        new RemoveCardFromHandCommand(this, playedCard).AddToQueue();

    }
    public void PlayASpellFromHand(string UniqueID, int laneIndex, int tablePos)
    {
        var playedCard = CardLogic.CardsCreatedThisGame[UniqueID];
        if (ManaLeft < playedCard.CurrentManaCost)
        {
            Debug.LogWarning(
                $"[MANA] Not enough mana to play {playedCard.ca.name}. Needed: {playedCard.CurrentManaCost}, have: {ManaLeft}");
            return;
        }

        new SpendManaCommand(this, playedCard.CurrentManaCost).AddToQueue();
        
        
        switch (playedCard.ca.TriggerAbilities)
        {
            case "A1":
            {
                Debug.Log($"[A1] {playedCard.ca.name}");
                new DrawACardCommand(this).AddToQueue();
                break;
            }
            case "A2":
            {
                Debug.Log($"[A2] {playedCard.ca.name}");
                new DrawACardCommand(this).AddToQueue();
                new DrawACardCommand(this).AddToQueue();
                // new AddMaxManaCommand(this,1).AddToQueue();
                break;
            }
        }
        
        
        new RemoveCardFromHandCommand(this, playedCard).AddToQueue();
        
    }

    public virtual void OnTurnStart()
    {
        foreach (var table in tables)
        {
            foreach (CreatureLogic cl in table.CreaturesOnTable)
                cl.OnTurnStart();
        }
    }

    public void OnTurnEnd()
    {
        EndTurnEvent?.Invoke();
        Debug.Log("[Turn tried to end turn");
        GetComponent<TurnMaker>().StopAllCoroutines();
    }

    //public event VoidWithNoArguments CreaturePlayedEvent;
    //public event VoidWithNoArguments SpellPlayedEvent;
    //public event VoidWithNoArguments StartTurnEvent;

    public void Die()
    {
        // game over
        // // block both players from taking new moves 
        // PArea.ControlsON = false;
        // otherPlayer.PArea.ControlsON = false;
        // TurnManager.Instance.StopTheTimer();
        // new GameOverCommand(this).AddToQueue();
    }

}