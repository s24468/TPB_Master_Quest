using UnityEngine;
using System.Collections;
using Cards;
using DG.Tweening;

public class DragCreatureOnIDropTarget : DraggingActions
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
        Debug.Log("Saving handslot number: "+ savedHandSlot);
        tempState = whereIsCard.VisualState;
        whereIsCard.VisualState = VisualStates.Dragging;
        whereIsCard.BringToFront();
    }

    public override void OnDraggingInUpdate()
    {
    }
    public override void OnEndDrag()
    {
        var target = GetHoveredDropTarget();

        if (target != null && target.CanAcceptDrop(this))
        {
            target.AcceptDrop(this);
            return;
        }

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
    protected IDropTarget GetHoveredDropTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, ~0, QueryTriggerInteraction.Collide);

        foreach (var h in hits)
        {
            if (h.collider == null) continue;

            // szukamy komponentu DropTarget na obiekcie collidera
            var dt = h.collider.GetComponent<DropTarget>() 
                     ?? h.collider.GetComponentInParent<DropTarget>();

            if (dt != null && dt.Target != null)
                return dt.Target;
        }

        return null;
    }

    protected override bool DragSuccessful()
    {
        var target = GetHoveredDropTarget();
        return target != null && target.CanAcceptDrop(this);
    }

}


    // public override void OnEndDrag()
    // {
    //     if (DragSuccessful())
    //     {
    //         // ===== 1️⃣ DROP NA MANA POOL =====
    //         if (Cards.ManaPoolVisual.HoveredManaPool != null)
    //         {
    //             var hoveredMana = Cards.ManaPoolVisual.HoveredManaPool;
    //
    //             var manaArea = hoveredMana.GetComponentInParent<PlayerArea>();
    //
    //             // bezpieczeństwo – nie można na enemy mana
    //             if (manaArea == null || manaArea.owner != playerOwner.PArea.owner)
    //             {
    //                 ReturnToHand();
    //                 return;
    //             }
    //
    //             // +1 mana przez command
    //             playerOwner.SacrificeCardForOneMaxMana(
    //                 GetComponent<IDHolder>().UniqueID
    //             );
    //
    //             // usuń wizualnie kartę z ręki
    //             HandVisual playerHand = playerOwner.PArea.handVisual;
    //
    //             GameObject card = Services
    //                 .Get<IInstanceIdService>()
    //                 .Find(GetComponent<IDHolder>().UniqueID);
    //
    //             playerHand.RemoveCard(card);
    //
    //             Destroy(gameObject);
    //             return;
    //         }
    //
    //         // ===== 2️⃣ DROP NA TABLE =====
    //         if (TableVisual.HoveredTable != null)
    //         {
    //             TableVisual hovered = TableVisual.HoveredTable;
    //
    //             if (hovered.owner != playerOwner.PArea.owner)
    //             {
    //                 ReturnToHand();
    //                 return;
    //             }
    //             string id = GetComponent<IDHolder>().UniqueID;
    //             var playedCard = CardLogic.CardsCreatedThisGame[id];
    //
    //             // ✅ NOWE: sprawdzenie many przy dropie
    //             if (playerOwner.ManaLeft < playedCard.CurrentManaCost)
    //             {
    //                 Debug.LogWarning($"[MANA] Not enough mana to play {playedCard.ca.name}. Needed: {playedCard.CurrentManaCost}, have: {playerOwner.ManaLeft}");
    //                 ReturnToHand();
    //                 return;
    //             }
    //
    //             float mouseX = Camera.main.ScreenToWorldPoint(
    //                 new Vector3(
    //                     Input.mousePosition.x,
    //                     Input.mousePosition.y,
    //                     transform.position.z - Camera.main.transform.position.z
    //                 )
    //             ).x;
    //
    //             int tablePos = hovered.TablePosForNewCreature(mouseX);
    //             int laneIndex = hovered.laneIndex;
    //
    //             playerOwner.PlayACreatureFromHand(
    //                 GetComponent<IDHolder>().UniqueID,
    //                 laneIndex,
    //                 tablePos
    //             );
    //
    //             return;
    //         }
    //     }
    //     ReturnToHand();
    // }

// bool TableNotFull = (playerOwner.table.CreaturesOnTable.Count < 8);


// public override void OnEndDrag()
// {
//     // 1) Check if we are holding a card over the table
//     if (DragSuccessful())
//     {
//         // determine table position
//         int tablePos = playerOwner.PArea.tableVisual.TablePosForNewCreature(Camera.main.ScreenToWorldPoint(
//             new Vector3(Input.mousePosition.x, Input.mousePosition.y,
//                 transform.position.z - Camera.main.transform.position.z)).x);
//         Debug.Log("Table Pos for new Creature: " + tablePos.ToString());
//         // play this card
//         playerOwner.PlayACreatureFromHand(GetComponent<IDHolder>().UniqueID, tablePos);
//     }
//     else
//     {
//         // Set old sorting order 
//         Debug.Log("Saved  handslot number: "+ savedHandSlot);
//
//         whereIsCard.SetHandSortingOrder();
//         Debug.Log("Saved  handslot number after setgandSortingOrder: "+ savedHandSlot);
//
//         whereIsCard.VisualState = tempState;
//         // Move this card back to its slot position
//         HandVisual PlayerHand = playerOwner.PArea.handVisual;
//         Debug.Log("I coulnt find table pos for new creature "+ PlayerHand.slots.children.Length.ToString() +" "+ savedHandSlot);
//         
//         
//         
//         Vector3 oldCardPos = PlayerHand.slots.children[savedHandSlot].transform.localPosition;
//         transform.DOLocalMove(oldCardPos, 1f);
//     }
// }