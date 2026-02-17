using UnityEngine;
using System.Collections;
using Cards;
using UnityEngine.Serialization;

public enum AreaPosition{Top, Low}

public class PlayerArea : MonoBehaviour 
{
    public AreaPosition owner;
    public bool ControlsON = true;
    public ManaPoolVisual ManaPool;
    [FormerlySerializedAs("handVisual")] public HandManager handManager;
    public TableVisual[] tableVisuals = new TableVisual[3];
}
