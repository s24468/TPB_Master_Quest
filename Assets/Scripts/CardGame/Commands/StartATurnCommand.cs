using UnityEngine;
using System.Collections;
using Cards;

public class StartATurnCommand : Command {

    private Player p;

    public StartATurnCommand(Player p)
    {
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        TurnManager.Instance.StartTurn(p);
        CommandExecutionComplete();
    }
}
