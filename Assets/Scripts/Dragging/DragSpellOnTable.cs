using UnityEngine;
using System.Collections;
using Cards;
using DG.Tweening;

public class DragSpellOnTable : DraggingActions
{
    private int savedHandSlot;
    private WhereIsTheCardOrCreature whereIsCard;
    private IDHolder idScript;
    private VisualStates tempState;
    private OneCardManager manager;

    public override bool CanDrag
    {
        get
        {
            // TODO : include full field check
            return base.CanDrag && manager.CanBePlayedNow;
        }
    }

    void Awake()
    {
        whereIsCard = GetComponent<WhereIsTheCardOrCreature>();
        manager = GetComponent<OneCardManager>();
    }

    public override void OnStartDrag()
    {
        savedHandSlot = whereIsCard.Slot;
        Debug.Log("Saving handslot number: " + savedHandSlot);


        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();
    }

    public override void OnDraggingInUpdate()
    {
    }


    public override void OnEndDrag()
    {
        // if (DragSuccessful())
        // {
        //     TableVisual hovered = TableVisual.HoveredTable;
        //
        //     // bezpieczeństwo: nie pozwól dropnąć na enemy stół
        //     if (hovered == null || hovered.owner != playerOwner.PArea.owner)
        //     {
        //         ReturnToHand();
        //         return;
        //     }
        //
        //     float mouseX = Camera.main.ScreenToWorldPoint(
        //         new Vector3(Input.mousePosition.x, Input.mousePosition.y,
        //             transform.position.z - Camera.main.transform.position.z)).x;
        //
        //     int tablePos = hovered.TablePosForNewCreature(mouseX);
        //     int laneIndex = hovered.laneIndex;
        //
        //     playerOwner.PlayACreatureFromHand(GetComponent<IDHolder>().UniqueID, laneIndex, tablePos);
        //     return;
        // }

        ReturnToHand();
    }

    private void ReturnToHand()
    {
        whereIsCard.SetHandSortingOrder();
        whereIsCard.VisualState = tempState;

        HandVisual PlayerHand = playerOwner.PArea.handVisual;
        Vector3 oldCardPos = PlayerHand.slots.children[savedHandSlot].transform.localPosition;
        transform.DOLocalMove(oldCardPos, 1f);
    }

    protected override bool DragSuccessful()
    {
        return TableVisual.CursorOverSomeTable;
    }
}