using UnityEngine;
using System.Collections;
using Cards;

public class DrawACardCommand : Command
{
    private Player player;

    public DrawACardCommand(Player player)
    {
        this.player = player;
    }

    public override void StartCommandExecution()
    {
        player.hand.GivePlayerARandomCard();
        CommandExecutionComplete();
    }
    // player.StartCoroutine(DrawCardAnimation());

    // private IEnumerator DrawCardAnimation()
    // {
    //     // odpal animację w HandVisual
    //     yield return player.hand
    //         .GetComponent<HandVisual>()
    //         .AnimateCardDraw(card);
    //
    //     CommandExecutionComplete();
    // }
}