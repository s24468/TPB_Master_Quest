using UnityEngine;
using Cards;

public class AddManaCrystalCommand : Command
{
    private Player player;
    private int amount;

    public AddManaCrystalCommand(Player player, int amount = 1)
    {
        this.player = player;
        this.amount = amount;
    }

    public override void StartCommandExecution()
    {
        player.ManaThisTurn += amount;

        player.ManaLeft = Mathf.Min(
            player.ManaLeft + amount,
            player.ManaThisTurn
        );

        if (player.PArea != null && player.PArea.ManaPool != null)
        {
            player.PArea.ManaPool.SetMax(player.ManaThisTurn);
            player.PArea.ManaPool.SetAvailable(player.ManaLeft);
        }

        CommandExecutionComplete();
    }
}