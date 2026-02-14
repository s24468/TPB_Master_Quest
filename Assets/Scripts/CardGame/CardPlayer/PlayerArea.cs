using UnityEngine;
using System.Collections;
using Cards;

public enum AreaPosition{Top, Low}

public class PlayerArea : MonoBehaviour 
{
    public AreaPosition owner;
    public bool ControlsON = true;
    // public PlayerDeckVisual PDeck;
    public ManaPoolVisual ManaBar;
    public HandVisual handVisual;
    // public EndTurnButton EndTurnButton;
    public TableVisual[] tableVisuals = new TableVisual[3]; // size 3

    public Transform PortraitPosition;

    public bool AllowedToControlThisPlayer
    {
        get;
        set;
    }      


}
