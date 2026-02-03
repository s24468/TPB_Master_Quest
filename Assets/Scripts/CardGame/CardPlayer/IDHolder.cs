using System;
using System.Collections.Generic;
using UnityEngine;

public class IDHolder : MonoBehaviour
{
    public string UniqueID;
    public static List<IDHolder> AllIDHolders = new List<IDHolder>();

    private void Awake()
    {
        AllIDHolders.Add(this);
    }

    public static GameObject GetGameObjectWithID(string ID)
    {
        foreach (var i in AllIDHolders)
        {
            if (i.UniqueID == ID)
            {
                return i.gameObject;
            }
        }
        return null;
    }
    private void OnDestroy()
    {
        // usuń z listy, gdy GO/komponent jest niszczony
        AllIDHolders.Remove(this);
    }
    public static void ClearIDHoldersList()
    {
        AllIDHolders.Clear();
    }
}