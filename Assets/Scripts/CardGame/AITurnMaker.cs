using Cards;
using UnityEngine;

public class AITurnMaker : TurnMaker
{
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        Debug.Log("AI turn started");
        // StartCoroutine(PerformAITurn());
    }
}