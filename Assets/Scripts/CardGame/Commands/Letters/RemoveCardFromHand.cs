using UnityEngine;
using Cards;

public class RemoveCardFromHand : Command
{
    private Player player;
    private int amount;

    public RemoveCardFromHand(Player player, int amount = 1)
    {
        this.player = player;
        this.amount = amount;
    }

    public override void StartCommandExecution()
    {
        player.hand.RemoveRandomCardsFromHandToCenterThenDestroy(
            player,
            amount,
            onEachCardComplete: () => player.PArea.ManaPool.AddMaxCrystals(1),
            onAllComplete: CommandExecutionComplete
        );
    }


}