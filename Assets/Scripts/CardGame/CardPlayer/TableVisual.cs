using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cards;
using DG.Tweening;
using NUnit.Framework;
using UI;

public class TableVisual : MonoBehaviour
{
    // PUBLIC FIELDS
    public int laneIndex; // 0..2
    public AreaPosition owner;
    public SameDistanceChildren slots;

    // PRIVATE FIELDS
    private List<GameObject> CreaturesOnTable = new List<GameObject>();
    private bool cursorOverThisTable = false;
    private BoxCollider col;
    public static TableVisual HoveredTable { get; private set; }

    public static bool CursorOverSomeTable => HoveredTable != null;
    // returns true if we are hovering over any player`s table collider
    // public static bool CursorOverSomeTable
    // {
    //     get
    //     {
    //         TableVisual[] bothTables = GameObject.FindObjectsOfType<TableVisual>();
    //         Debug.Log(bothTables.Length);
    //         if (bothTables.Length > 0)
    //             Debug.Log(bothTables[0].name);
    //
    //         return (bothTables[0].CursorOverThisTable) || bothTables[1].CursorOverThisTable;
    //     }
    // }

    // returns true only if we are hovering over this table`s collider
    public bool CursorOverThisTable
    {
        get { return cursorOverThisTable; }
    }

    // METHODS

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    // CURSOR/MOUSE DETECTION
    // void Update()
    // {
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //     // Zasięg nieskończony, uwzględniamy wszystkie layer’y oraz triggery
    //     RaycastHit[] hits = Physics.RaycastAll(
    //         ray,
    //         Mathf.Infinity,
    //         ~0,
    //         QueryTriggerInteraction.Collide
    //     );
    //
    //     bool passedThroughTableCollider = false;
    //
    //     foreach (RaycastHit h in hits)
    //     {
    //         // check if the collider that we hit is the collider on this GameObject
    //         if (h.collider == col)
    //             passedThroughTableCollider = true;
    //     }
    //
    //     cursorOverThisTable = passedThroughTableCollider;
    // }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);

        bool passed = false;
        foreach (var h in hits)
            if (h.collider == col) { passed = true; break; }

        cursorOverThisTable = passed;

        // zarządzanie globalnym "hover"
        if (passed)
            HoveredTable = this;
        else if (HoveredTable == this)
            HoveredTable = null;
    }

    public void AddCreatureAtIndex(CardAsset ca, string UniqueID, int index, Vector3 eulerAngles)
    {
        GameObject creature = Instantiate(GlobalSettings.Instance.CreaturePrefab,
            slots.children[index].transform.position, Quaternion.Euler(eulerAngles));
        creature.transform.localScale = new Vector3(8f, 8f, 1f);
        creature.transform.rotation = Quaternion.Euler(0, 0, 0);


        OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
        manager.cardAsset = ca;
        manager.ReadCreatureFromAsset();

        foreach (Transform t in creature.GetComponentsInChildren<Transform>())
        {
            t.tag = owner.ToString() + "Creature";
        }
        // t.tag = "Creature";

        // parent a new creature gameObject to table slots
        creature.transform.SetParent(slots.transform);

        CreaturesOnTable.Insert(index, creature);

        WhereIsTheCardOrCreature w = creature.GetComponent<WhereIsTheCardOrCreature>();
        w.Slot = index;
        // w.VisualState = VisualStates.LowTable;

        // add our unique ID to this creature
        IDHolder id = creature.AddComponent<IDHolder>();
        id.UniqueID = UniqueID;

        // after a new creature is added update placing of all the other creatures
        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();

        // end command execution
        Command.CommandExecutionComplete();
    }


    public int TablePosForNewCreature(float MouseX)
    {
        if (CreaturesOnTable.Count == 0 || MouseX > slots.children[0].transform.position.x)
        {
            return 0;
        }
        else if (MouseX < slots.children[CreaturesOnTable.Count - 1].transform.position
                     .x) // cursor on the left relative to all creatures on the table
        {
            return CreaturesOnTable.Count;
        }

        for (int i = 0; i < CreaturesOnTable.Count; i++)
        {
            if (MouseX < slots.children[i].transform.position.x && MouseX > slots.children[i + 1].transform.position.x)
                return i + 1;
        }

        Debug.Log("Suspicious behavior. Reached end of TablePosForNewCreature method. Returning 0");
        return 0;
    }

    // Destroy a creature
    public void RemoveCreatureWithID(string IDToRemove)
    {
        GameObject creatureToRemove = IDHolder.GetGameObjectWithID(IDToRemove);
        CreaturesOnTable.Remove(creatureToRemove);
        Destroy(creatureToRemove);
        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();
        Command.CommandExecutionComplete();
    }

    void ShiftSlotsGameObjectAccordingToNumberOfCreatures()
    {
        float targetX = 0f;

        if (CreaturesOnTable.Count > 0)
        {
            float right = slots.children[0].localPosition.x;
            float left = slots.children[CreaturesOnTable.Count - 1].localPosition.x;

            float mid = (right + left) * 0.5f;
            targetX = -mid;
        }

        slots.transform.DOLocalMoveX(targetX, 0.3f);
    }

    void PlaceCreaturesOnNewSlots()
    {
        for (int i = 0; i < CreaturesOnTable.Count; i++)
        {
            GameObject g = CreaturesOnTable[i];

            var slot = slots.children[i];

            Debug.Log(
                $"[TABLE] i={i} | slot local={slot.localPosition} world={slot.position} | " +
                $"creature local={g.transform.localPosition} world={g.transform.position} | " +
                $"slots scale={slots.transform.lossyScale}"
            );

            g.transform.DOLocalMove(slot.localPosition, 0.3f);
        }
    }
}