using UnityEngine;
using Cards;

public class AddMaxManaCommand : Command
{
    private Player player;
    private int amount;

    public AddMaxManaCommand(Player player, int amount = 1)
    {
        this.player = player;
        this.amount = amount;
    }

    public override void StartCommandExecution()
    {
        player.PArea.ManaPool.AddMaxCrystals(amount);
        CommandExecutionComplete();
    }
}