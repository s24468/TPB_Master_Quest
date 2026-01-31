using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TargetingOptions
{
    NoTarget,
    AllCreatures, 
    EnemyCreatures,
    YourCreatures, 
    AllCharacters, 
    EnemyCharacters,
    YourCharacters
}

public class CardAsset : ScriptableObject 
{
    [Header("General info")]
	public string Id;
	public string Name;
	public Sprite CardImage;
    public int ManaCost;
    public bool IsCreature;
    [TextArea(2,3)]
    public string Description;  // Description for spell or character

    [Header("Creature Info")]
    public int CasualPower;
    public int TPower;
    public int PPower;
    public int BPower;

    [Header("SpellInfo")]
    public TargetingOptions Targets;
}
