using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HoverOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite newSprite;
    public Sprite originalSprite;
    public Image buttonImage;
    [SerializeField] private Color hoverColor = new Color(1f, 0.086f, 0f, 1f);
    [SerializeField] private Color mainColor = new Color(0.086f, 0.714f, 0.694f, 1f);
    private Light[] _lights;

    void Start()
    {
        _lights = GetComponentsInChildren<Light>();
    }

    private void OnEnable()
    {
        buttonImage.sprite = originalSprite;
        if (_lights != null)
        {

            foreach (var light in _lights)
            {
                light.color = mainColor;
            }            
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonImage.sprite = newSprite;
        foreach (var light in _lights)
        {
            light.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonImage.sprite = originalSprite;
        foreach (var light in _lights)
        {
            light.color = mainColor;
        }
    }
}