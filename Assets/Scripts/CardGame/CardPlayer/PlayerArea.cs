using UnityEngine;
using System.Collections;
using Cards;
using UnityEngine.Serialization;

public enum AreaPosition{Top, Low}

public class PlayerArea : MonoBehaviour 
{
    public AreaPosition owner;
    public bool ControlsON = true;
    // public PlayerDeckVisual PDeck;
    // public EndTurnButton EndTurnButton;
    public ManaPoolVisual ManaPool;
    public HandVisual handVisual;
    public TableVisual[] tableVisuals = new TableVisual[3];

    public Transform PortraitPosition;

    public bool AllowedToControlThisPlayer
    {
        get;
        set;
    }      

}
