using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite newSprite;
    public Sprite originalSprite;
    public Image buttonImage;

    private Light[] _lights;

    void Start()
    {
        _lights = GetComponentsInChildren<Light>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = newSprite;
        foreach (var light in _lights)
        {
            light.color = new Color(255f / 255f, 22f / 255f, 0f / 255f, 255f / 255f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = originalSprite;
        foreach (var light in _lights)
        {
            light.color = new Color(22f / 255f, 182f / 255f, 177f / 255f, 255f / 255f);
        }
    }
}