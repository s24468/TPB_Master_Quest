using UnityEngine;
using System.Collections;
using DG.Tweening;

public class DelayCommand : Command
{

    private readonly float delay;

    public DelayCommand(float delay)
    {
        this.delay = delay;
    }

    public override void StartCommandExecution()
    {
        DOVirtual.DelayedCall(delay, () => { CommandExecutionComplete(); });
    }
}