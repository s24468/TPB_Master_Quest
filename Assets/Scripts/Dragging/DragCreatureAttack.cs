using UnityEngine;
using Cards;
using Dragging;

public class DragCreatureAttack : DraggingActions
{
    private SpriteRenderer _sr;

    private LineRenderer _lr;

    private WhereIsTheCardOrCreature _whereIsThisCreature;

    private Transform _triangle;

    private SpriteRenderer _triangleSr;

    private GameObject _target;

    private OneCreatureManager _manager;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _lr = GetComponentInChildren<LineRenderer>();
        _lr.sortingLayerName = "AboveEverything";
        _triangle = transform.Find("Triangle");
        _triangleSr = _triangle.GetComponent<SpriteRenderer>();

        _manager = GetComponentInParent<OneCreatureManager>();
        _whereIsThisCreature = GetComponentInParent<WhereIsTheCardOrCreature>();
    }

    public override bool CanDrag
    {
        get
        {
            // we can drag this card if 
            // a) we can control this our player (this is checked in base.canDrag)
            // b) creature "CanAttackNow" - this info comes from logic part of our code into each creature`s manager script
            return base.CanDrag && _manager.CanAttackNow;
        }
    }

    public override void OnStartDrag()
    {
        _whereIsThisCreature.VisualState = VisualStates.Dragging;
        _sr.enabled = true;
        _lr.enabled = true;
    }

    public override void OnDraggingInUpdate()
    {
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;
        if (notNormalized.magnitude > distanceToTarget)
        {
            // draw a line between the creature and the target
            _lr.SetPositions(new Vector3[] { transform.parent.position, transform.position - direction * 2.3f });
            _lr.enabled = true;

            // position the end of the arrow between near the target.
            _triangleSr.enabled = true;
            _triangleSr.transform.position = transform.position - 1.5f * direction;

            // proper rotarion of arrow end
            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            _triangleSr.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            // if the target is not far enough from creature, do not show the arrow
            _lr.enabled = false;
            _triangleSr.enabled = false;
        }
    }

    public override void OnEndDrag()
    {
        Debug.Log("End dragging");

        // 1) szukamy targetu (IAttackable)
        IAttackable targetAttackable = null;

        var origin = Camera.main.transform.position;
        var dir = (transform.position - origin).normalized;

        RaycastHit[] hits = Physics.RaycastAll(origin, dir, 200f);
        Debug.Log($"Raycast hits: {hits.Length}");

        foreach (var h in hits)
        {
            // ignoruj samego siebie i swoje dzieci
            if (h.transform == transform || h.transform.IsChildOf(transform))
            {
                continue;
            }

            // bierz pierwszy obiekt, który implementuje IAttackable (np. turret albo creature)
            var candidate = h.transform.GetComponentInParent<IAttackable>();
            if (candidate == null)
            {
                continue;
            }

            var attackerId = GetComponentInParent<IDHolder>().UniqueID;
            var attackerOwnerId = CreatureLogic.CreaturesCreatedThisGame[attackerId].owner.PlayerID;

            if (candidate.Owner != null && candidate.Owner.PlayerID == attackerOwnerId)
                continue;


            targetAttackable = candidate;
            break;
        }

        bool targetValid = false;

        // 2) walidacja + wykonanie ataku
        if (targetAttackable != null)
        {
            // attackerId (UniqueID tej creatury)
            var attackerHolder = GetComponentInParent<IDHolder>();
            string attackerId = attackerHolder != null ? attackerHolder.UniqueID : string.Empty;

            Debug.Log(
                $"Target picked: {(targetAttackable as MonoBehaviour)?.name}, targetID: {targetAttackable.UniqueID}");

            targetAttackable.ReceiveAttack(attackerId);
            targetValid = true;
        }

        // 3) jeśli nieważny target -> wróć
        if (!targetValid)
        {
            _whereIsThisCreature.VisualState = VisualStates.LowTable;
            _whereIsThisCreature.SetTableSortingOrder();
        }

        // 4) zawsze resetuj wizual
        transform.localPosition = Vector3.zero;
        _sr.enabled = false;
        _lr.enabled = false;
        _triangleSr.enabled = false;
    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}