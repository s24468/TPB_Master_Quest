using UnityEngine;
using System.Collections;

public class CreatureDieCommand : Command 
{
    private Player p;
    private string DeadCreatureID;
    
    public CreatureDieCommand(string CreatureID, Player p)
    {
        this.p = p;
        this.DeadCreatureID = CreatureID;
    }
    
    public override void StartCommandExecution()
    {
        p.PArea.tableVisual.RemoveCreatureWithID(DeadCreatureID);
    }
}
