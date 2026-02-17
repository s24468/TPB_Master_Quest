public class RefillManaPoolCommand : Command
{
    private Player p;

    public RefillManaPoolCommand(Player p)
    {
        this.p = p;
    }

    public override void StartCommandExecution()
    {
        p.PArea.ManaPool.RefillAll();
        CommandExecutionComplete();
    }
}