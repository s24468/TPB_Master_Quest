public class CreatureDieCommand : Command
{
    private Player p;
    private string deadCreatureID;
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