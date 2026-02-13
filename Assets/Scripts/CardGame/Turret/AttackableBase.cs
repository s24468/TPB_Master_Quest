using Dragging;
using UnityEngine;

public abstract class AttackableBase : MonoBehaviour, IAttackable
{
    [SerializeField] private Player owner;

    public virtual Player Owner => owner;

    public virtual string UniqueID
    {
        get
        {
            var holder = GetComponentInParent<IDHolder>();
            return holder != null ? holder.UniqueID : string.Empty;
        }
    }

    public abstract void ReceiveAttack(string attackerId);
}