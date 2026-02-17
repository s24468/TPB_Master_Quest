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


    [SerializeField] private LayerMask targetMask;
    private readonly RaycastHit[] _hits = new RaycastHit[8];
    private Targetable _currentTarget;
    private float _nextCheckTime;
    [SerializeField] private float targetCheckInterval = 0.03f;

    private string _attackerId;
    private int _attackerOwnerId;
    private bool _hasCachedAttacker;
    private Biome _attackerBiome;
    private bool _hasAttackerBiome;
    private CreatureLogic _logic;
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
    
    //
    public override bool CanDrag
    {
        get
        {
            if (!base.CanDrag) return false;
            if (!TryCacheAttacker()) return false;
            return _logic.CanAttack; // AttacksLeftThisTurn > 0 && !Frozen && ownersTurn
        }
    }

    private bool TryCacheAttacker()
    {
        if (_logic != null && !string.IsNullOrEmpty(_attackerId))
        {
            // odśwież biome (bo table może się zmienić)
            _attackerBiome = _logic.CurrentBiome;
            _hasAttackerBiome = true;
            return true;
        }

        _attackerId = GetComponentInParent<IDHolder>()?.UniqueID ?? string.Empty;
        if (string.IsNullOrEmpty(_attackerId))
        {
            _hasAttackerBiome = false;
            return false;
        }

        if (CreatureLogic.CreaturesCreatedThisGame.TryGetValue(_attackerId, out var logic) && logic != null)
        {
            _logic = logic;
            _attackerBiome = logic.CurrentBiome;
            _hasAttackerBiome = true;
            return true;
        }

        _hasAttackerBiome = false;
        return false;
    }
    public override void OnStartDrag()
    {
        // _attackerId = GetComponentInParent<IDHolder>()?.UniqueID;
        // if (!string.IsNullOrEmpty(_attackerId) &&
        //     CreatureLogic.CreaturesCreatedThisGame.TryGetValue(_attackerId, out var logic))
        // {
        //     _logic = logic;
        //     _attackerBiome = logic.CurrentBiome;
        //     _hasAttackerBiome = true;
        // }
        // else
        // {
        //     _hasAttackerBiome = false;
        // }

        if (!TryCacheAttacker() || !_logic.CanAttack)
            return;
        if (_logic == null )
        {
            return;
        }

        if (!_logic.CanAttack)
        {
            return;
        }

        _whereIsThisCreature.VisualState = VisualStates.Dragging;
        _sr.enabled = true;
        _lr.enabled = true;
    }
    private bool TryGetTargetBiome(IAttackable target, out Biome biome)
    {
        biome = default;

        if (target == null) return false;

        // 1) Turret
        if (target is TurretController turret)
        {
            biome = turret.Biome;
            return true;
        }

        // 2) Creature (po UniqueID -> CreatureLogic -> CurrentBiome)
        var id = target.UniqueID;
        if (!string.IsNullOrEmpty(id) &&
            CreatureLogic.CreaturesCreatedThisGame.TryGetValue(id, out var logic) &&
            logic != null)
        {
            biome = logic.CurrentBiome;
            return true;
        }

        return false;
    }

    public override void OnDraggingInUpdate()
    {
        // 1) arrow visuals (Twoje, bez zmian)
        Vector3 notNormalized = transform.position - transform.parent.position;
        Vector3 direction = notNormalized.normalized;
        float distanceToTarget = (direction * 2.3f).magnitude;

        bool farEnough = notNormalized.magnitude > distanceToTarget;

        _lr.enabled = farEnough;
        _triangleSr.enabled = farEnough;

        if (farEnough)
        {
            _lr.SetPositions(new[] { transform.parent.position, transform.position - direction * 2.3f });
            _triangleSr.transform.position = transform.position - 1.5f * direction;

            float rot_z = Mathf.Atan2(notNormalized.y, notNormalized.x) * Mathf.Rad2Deg;
            _triangleSr.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }

        // 2) highlight update co X sekund
        if (Time.time < _nextCheckTime)
        {
            return;
        }

        _nextCheckTime = Time.time + targetCheckInterval;

        var newTarget = RaycastTargetable();
        SetHighlighted(newTarget);
    }

    public override void OnEndDrag()
    {
        // target z ostatniego update (wystarczy w praktyce)
        var target = _currentTarget?.Attackable;

        bool targetValid = IsValidTarget(target);

        if (targetValid && target != null)
        {
            target.ReceiveAttack(_attackerId);
        }
        else
        {
            _whereIsThisCreature.VisualState = VisualStates.LowTable;
            _whereIsThisCreature.SetTableSortingOrder();
        }

        // reset wizual
        transform.localPosition = Vector3.zero;
        _sr.enabled = false;
        _lr.enabled = false;
        _triangleSr.enabled = false;
        if (_currentTarget?.Glow != null)
        {
            _currentTarget.Glow.Hide();
        }
        _currentTarget = null;
        _hasCachedAttacker = false;
    }


    private Targetable RaycastTargetable()
    {
        var cam = Camera.main;
        if (cam == null)
        {
            return null;
        }

        var origin = cam.transform.position;
        var dir = (transform.position - origin).normalized;

        int count = Physics.RaycastNonAlloc(origin, dir, _hits, 200f, targetMask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            var col = _hits[i].collider;
            if (!col)
            {
                continue;
            }

            // ignoruj siebie i swoje dzieci
            if (col.transform == transform || col.transform.IsChildOf(transform))
            {
                continue;
            }

            var t = col.GetComponent<Targetable>();
            if (t != null && t.Attackable != null)
            {
                return t;
            }
        }

        return null;
    }

    private bool IsValidTarget(IAttackable target)
    {
        Debug.Log("AAAAAAA");
        if (target == null) return false;
        Debug.Log("BBBB");

        // owner check (Twoja wersja z playerOwner)
        var me = playerOwner;
        if (me == null) return false;
        Debug.Log("CCCC");

        var them = target.Owner;
        if (them != null && them.PlayerID == me.PlayerID)
            return false;
        Debug.Log("DDDD");

        // biome check
        if (!_hasAttackerBiome) return false;

        Debug.Log("EEEEEE");

        if (!TryGetTargetBiome(target, out var targetBiome))
            return false;
        Debug.Log("FFFFF");

        return targetBiome == _attackerBiome;
    }


    private Color GetGlowColorFor(IAttackable target)
    {
        return IsValidTarget(target)
            ? new Color(0.2f, 2.2f, 0.2f, 1f) // zieleń
            : new Color(2.2f, 0.1f, 0.1f, 1f); // czerwień
    }

    private void SetHighlighted(Targetable newTarget)
    {
        if (newTarget == _currentTarget)
        {
            return;
        }

        // zgaś poprzedni
        if (_currentTarget?.Glow != null)
        {
            _currentTarget.Glow.Hide();
        }

        _currentTarget = newTarget;

        // zapal nowy
        if (_currentTarget?.Glow != null)
        {
            _currentTarget.Glow.SetColor(GetGlowColorFor(_currentTarget.Attackable));
            _currentTarget.Glow.Show();
        }
    }

    // NOT USED IN THIS SCRIPT
    protected override bool DragSuccessful()
    {
        return true;
    }
}