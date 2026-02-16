using UnityEngine;
using Cards;

public class UpdateManaPoolCommand : Command
{
    private Player p;
    private int totalMana;
    private int availableMana;

    public UpdateManaPoolCommand(Player p, int totalMana, int availableMana)
    {
        this.p = p;
        this.totalMana = totalMana;
        this.availableMana = availableMana;
    }

    public override void StartCommandExecution()
    {
        if (p.PArea != null && p.PArea.ManaPool != null)
        {
            p.PArea.ManaPool.SetMax(totalMana);
            p.PArea.ManaPool.SetAvailable(availableMana);
        }

        CommandExecutionComplete();
    }
}