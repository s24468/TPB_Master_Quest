using UnityEngine;
using System.Collections;

// public class CreatureDieCommand : Command 
// {
//     private Player p;
//     private string DeadCreatureID;
//     
//     public CreatureDieCommand(string CreatureID, Player p)
//     {
//         this.p = p;
//         this.DeadCreatureID = CreatureID;
//     }
//     
//     public override void StartCommandExecution()
//     {
//         p.PArea.tableVisual.RemoveCreatureWithID(DeadCreatureID);
//     }
// }
using UnityEngine;

public class CreatureDieCommand : Command
{
    private Player p;
    private string deadCreatureID;

    // NEW:
    private int laneIndex;

    public CreatureDieCommand(string creatureID, Player p, int laneIndex)
    {
        this.p = p;
        this.deadCreatureID = creatureID;
        this.laneIndex = laneIndex;
    }

    public override void StartCommandExecution()
    {
        p.PArea.tableVisuals[laneIndex].RemoveCreatureWithID(deadCreatureID);
    }
}
