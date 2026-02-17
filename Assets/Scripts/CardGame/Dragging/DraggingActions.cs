using UnityEngine;
using System.Collections;

public abstract class DraggingActions : MonoBehaviour
{
    public abstract void OnStartDrag();

    public abstract void OnEndDrag();

    public abstract void OnDraggingInUpdate();


    public virtual bool CanDrag => true;
    // public abs bool CanDrag;
    public string DraggedUniqueID => GetComponent<IDHolder>().UniqueID;

    public virtual void ConsumeFromHandAndDestroy()
    {
        // usuń logicznie z hand (już robisz to w Player.Sacrifice..., więc tu tylko visual)
        var handVisual = playerOwner.PArea.handManager;

        GameObject cardObj = Services.Get<IInstanceIdService>().Find(DraggedUniqueID);
        if (cardObj != null)
            handVisual.RemoveCard(cardObj);

        Destroy(gameObject);
    }

    public virtual Player playerOwner
    {
        get
        {
            Debug.Log("Object detected!" +
                      "\nName: " + gameObject.name +
                      "\nTag: " + gameObject.tag);

            if (tag.Contains("Low"))
                return GlobalSettings.Instance.LowPlayer;
            else if (tag.Contains("Top"))
            {
                return GlobalSettings.Instance.TopPlayer;
            }

            else
            {
                Debug.LogError("Untagged Card or creature " + transform.parent.name);
                return null;
            }
        }
    }

    // public virtual bool CanDrag
    // {
    //     get
    //     {            
    //         return GlobalSettings.Instance.CanControlThisPlayer(playerOwner);
    //     }
    // }
    protected abstract bool DragSuccessful();
}