using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Light pointLeft;
    public Light pointRight;

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        pointLeft.color =  new Color(255f / 255f, 22f / 255f, 0f / 255f, 255f / 255f);
        pointRight.color =  new Color(255f / 255f, 22f / 255f, 0f / 255f, 255f / 255f);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointLeft.color =  new Color(22f / 255f, 182f / 255f, 177f / 255f, 255f / 255f);
        pointRight.color =  new Color(22f / 255f, 182f / 255f, 177f / 255f, 255f / 255f);
    }
}