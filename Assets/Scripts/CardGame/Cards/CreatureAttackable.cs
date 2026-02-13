using Dragging;
using UnityEngine;

public class CreatureAttackable : MonoBehaviour, IAttackable
{
    public string UniqueID
    {
        get
        {
            var h = GetComponentInParent<IDHolder>();
            return h != null ? h.UniqueID : string.Empty;
        }
    }

    public Player Owner
    {
        get
        {
            // tak jak u Ciebie: tag "Low"/"Top"
            if (tag.Contains("Low")) return GlobalSettings.Instance.LowPlayer;
            if (tag.Contains("Top")) return GlobalSettings.Instance.TopPlayer;
            return null;
        }
    }

    public void ReceiveAttack(string attackerId)
    {
        // attackerId atakuje tę creaturę (UniqueID = targetID)
        if (string.IsNullOrEmpty(attackerId) || string.IsNullOrEmpty(UniqueID))
            return;

        CreatureLogic.CreaturesCreatedThisGame[attackerId].AttackCreatureWithID(UniqueID);
    }
}