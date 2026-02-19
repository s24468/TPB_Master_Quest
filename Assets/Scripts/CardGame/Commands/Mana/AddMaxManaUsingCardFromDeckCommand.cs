using UnityEngine;
using Cards;

public class AddMaxManaUsingCardFromDeckCommand : Command
{
    private Player player;
    private int amount;

    public AddMaxManaUsingCardFromDeckCommand(Player player, int amount = 1)
    {
        this.player = player;
        this.amount = amount;
    }

    // public override void StartCommandExecution()
    // {
    //     Player p = player;
    //
    //     player.hand.GivePlayerARandomCardCenterThenTarget(
    //         p,
    //         onComplete: () =>
    //         {
    //             player.PArea.ManaPool.AddMaxCrystals(amount);
    //             CommandExecutionComplete();
    //         });
    // }
    public override void StartCommandExecution()
    {
        player.hand.GivePlayerARandomCardsCenterThenTarget(
            player,
            amount,
            onEachCardComplete: () => player.PArea.ManaPool.AddMaxCrystals(1),
            onAllComplete: CommandExecutionComplete
        );
    }


}