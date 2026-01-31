using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Table : MonoBehaviour 
{
    // public List<CreatureLogic> CreaturesOnTable = new List<CreatureLogic>();
    public List<CardLogic> CreaturesOnTable = new List<CardLogic>();

    public void PlaceCreatureAt(int index, CardLogic creature)
    {
        CreaturesOnTable.Insert(index, creature);
    }
        
}