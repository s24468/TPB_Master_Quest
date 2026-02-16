using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cards;
using DG.Tweening;
using NUnit.Framework;
using UI;

public class TableVisual : MonoBehaviour, IDropTarget
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

    public bool CursorOverThisTable
    {
        get { return cursorOverThisTable; }
    }

    // METHODS

    void Awake()
    {
        col = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);

        bool passed = false;
        foreach (var h in hits)
            if (h.collider == col)
            {
                passed = true;
                break;
            }

        cursorOverThisTable = passed;

        // zarządzanie globalnym "hover"
        if (passed)
            HoveredTable = this;
        else if (HoveredTable == this)
            HoveredTable = null;
    }

    public CreatureLogic AddCreatureAtIndex(CardLogic cardLogic, int index, Vector3 eulerAngles, Player PlayerOwner)
    {
        GameObject creature = Instantiate(GlobalSettings.Instance.CreaturePrefab,
            slots.children[index].transform.position, Quaternion.Euler(eulerAngles));
        creature.transform.localScale = new Vector3(8f, 8f, 1f);

        creature.transform.rotation = Quaternion.Euler(0, 0, 0);


        OneCreatureManager manager = creature.GetComponent<OneCreatureManager>();
        manager.cardAsset = cardLogic.ca;

        manager.ReadCreatureFromAsset();

        manager.CreatureLogic = new CreatureLogic(PlayerOwner, cardLogic, creature.GetComponent<IDHolder>().UniqueID,
            laneIndex, PlayerOwner.tables[laneIndex]);

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

        // after a new creature is added update placing of all the other creatures
        ShiftSlotsGameObjectAccordingToNumberOfCreatures();
        PlaceCreaturesOnNewSlots();

        // end command execution
        Command.CommandExecutionComplete();
        return manager.CreatureLogic;
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
        GameObject creatureToRemove = Services.Get<IInstanceIdService>().Find(IDToRemove);
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

    public bool CanAcceptDrop(DraggingActions dragged)
    {
        string id = dragged.DraggedUniqueID;
        var card = CardLogic.CardsCreatedThisGame[id];
        if (owner != dragged.playerOwner.PArea.owner)
        {
            return false;
        }

        if (dragged.playerOwner.ManaLeft < card.CurrentManaCost)
        {
            Debug.LogWarning($"[MANA] Not enough mana to play {card.ca.name}. Needed: {card.CurrentManaCost}, have: {dragged.playerOwner.ManaLeft}");
            return false;
        }

        return true;
    }

    public void AcceptDrop(DraggingActions dragged)
    {
        if (dragged is not DragCreatureOnIDropTarget)
        {
            return;
        }

        string id = dragged.DraggedUniqueID;

        float mouseX = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                dragged.transform.position.z - Camera.main.transform.position.z)).x;

        int tablePos = TablePosForNewCreature(mouseX);
        dragged.playerOwner.PlayACreatureFromHand(id, laneIndex, tablePos);
    }
}