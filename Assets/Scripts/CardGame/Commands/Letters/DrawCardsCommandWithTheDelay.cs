using UnityEngine;
using System.Collections;
using Cards;

using DG.Tweening;
using Cards;

public class DrawCardsCommandWithTheDelay : Command
{
    private readonly Player player;
    private readonly float startDelay;
    private readonly int numberOfCards;
    private readonly float intervalBetweenCards;

    private int remaining;

    public DrawCardsCommandWithTheDelay(
        Player player,
        float startDelay = 0f,
        int numberOfCards = 1,
        float intervalBetweenCards = 0f)
    {
        this.player = player;
        this.startDelay = startDelay;
        this.numberOfCards = Mathf.Max(1, numberOfCards);
        this.intervalBetweenCards = Mathf.Max(0f, intervalBetweenCards);
    }

    public override void StartCommandExecution()
    {
        remaining = numberOfCards;
        DrawNext(withDelay: startDelay);
    }

    private void DrawNext(float withDelay)
    {
        player.hand.GivePlayerARandomCardWithDelay(withDelay, () =>
        {
            remaining--;
            if (remaining <= 0)
            {
                CommandExecutionComplete();
                return;
            }

            // następna karta po odstępie
            if (intervalBetweenCards <= 0f)
                DrawNext(withDelay: 0f);
            else
                DrawNext(withDelay: intervalBetweenCards);
        });
    }
}
