using UnityEngine;
using UnityEngine.Serialization;

namespace Cards
{

    public class BetterCardRotation : MonoBehaviour {
    
    [FormerlySerializedAs("CardFront")] public RectTransform cardFront;
    
    [FormerlySerializedAs("CardBack")] public RectTransform cardBack;
    
    public Transform targetFacePoint;
    
    public Collider col;
    
    private bool _showingBack = false;
    
    void Update () 
    {
        var hits = Physics.RaycastAll(origin: Camera.main.transform.position, 
            direction: (-Camera.main.transform.position + targetFacePoint.position).normalized, 
            maxDistance: (-Camera.main.transform.position + targetFacePoint.position).magnitude);
        bool passedThroughColliderOnCard = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider == col)
                passedThroughColliderOnCard = true;
        }
        if (passedThroughColliderOnCard!= _showingBack)
        {
            _showingBack = passedThroughColliderOnCard;
            if (_showingBack)
            {
                cardFront.gameObject.SetActive(false);
                cardBack.gameObject.SetActive(true);
            }
            else
            {
                cardFront.gameObject.SetActive(true);
                cardBack.gameObject.SetActive(false);
            }
    
        }
    
    }
    }
}
