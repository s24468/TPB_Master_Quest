using UnityEngine;
using Cards;

public class SpendManaCommand : Command
{
    private Player p;
    private int costMana;

    public SpendManaCommand(Player p, int costMana)
    {
        this.p = p;
        this.costMana = costMana;
    }

    public override void StartCommandExecution()
    {
        p.PArea.ManaPool.SubtractAvailableCrystals(costMana);
        CommandExecutionComplete();
    }
}