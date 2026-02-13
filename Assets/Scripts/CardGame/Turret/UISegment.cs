using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UISegment : MonoBehaviour
{
    [SerializeField] public Image img;

    [SerializeField] private string glowStrengthParam = "_GlowStrength";
    [SerializeField] private string glowColorParam = "_GlowColor";

    [SerializeField] private float strongStrength = 3f;
    [SerializeField] private float weakStrength = 0.6f;

    [ColorUsage(true, true)] [SerializeField] public Color strongColor = new Color(0.1f, 3.5f, 3.5f, 1f);
    [ColorUsage(true, true)] [SerializeField] public Color weakColor   = new Color(0.05f, 0.35f, 0.35f, 1f);

    private Material _mat;
    // private void Awake()
    // {
    //     // // KLUCZ: robimy kopię materiału, żeby każdy segment miał własny
    //     // _mat = new Material(img.material);
    //     // img.material = _mat;
    //     // KLUCZ: robimy kopię materiału, żeby każdy segment miał własny
    //     _mat = new Material(material);
    //     img.material = _mat;
    // }
    public void SetMaterial(Material mat)
    {
        _mat = new Material(mat);
        img.material = _mat;
    }
    
    public void SetSprite(Sprite sprite)
    {
        img.sprite = sprite;
    }

    public void SetStrong()
    {
        _mat.SetFloat(glowStrengthParam, strongStrength);
        _mat.SetColor(glowColorParam, strongColor);
    }

    public void SetWeak()
    {
        _mat.SetFloat(glowStrengthParam, weakStrength);
        _mat.SetColor(glowColorParam, weakColor);
    }

    public void SetOff()
    {
        _mat.SetFloat(glowStrengthParam, 0f);
    }
}