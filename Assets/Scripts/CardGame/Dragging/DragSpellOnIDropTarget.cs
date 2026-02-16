using UnityEngine;
using System.Collections;
using Cards;
using DG.Tweening;

public class DragSpellOnIDropTarget : DraggingActions
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
        var target = GetHoveredDropTarget();

        // ✅ Tylko ManaPool na razie
        if (target is Cards.ManaPoolVisual manaTarget && target.CanAcceptDrop(this))
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
        // teraz sukces = istnieje target i akceptuje drop
        var target = GetHoveredDropTarget();
        return target != null && target.CanAcceptDrop(this);
    }
}
