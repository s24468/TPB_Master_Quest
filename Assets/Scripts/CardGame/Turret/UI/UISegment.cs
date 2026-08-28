using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UISegment : MonoBehaviour
{
    [SerializeField] public Image img;

    [SerializeField] private string glowStrengthParam = "_GlowStrength";
    [SerializeField] private string glowColorParam = "_GlowColor";
    // [SerializeField] private string strongColorValue = "_GlowColor";

    [SerializeField] public float strongStrength = 3f;
    [SerializeField] private float weakStrength = 0.6f;

    [ColorUsage(true, true)] [SerializeField] public Color strongColor = new Color(0.1f, 3.5f, 3.5f, 1f);
    [ColorUsage(true, true)] [SerializeField] public Color weakColor   = new Color(0.05f, 0.35f, 0.35f, 1f);

    private Material _mat;
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
// using UnityEngine;
// using UnityEngine.UI;
//
// public class UISegment : MonoBehaviour
// {
//     [SerializeField] public Image img;
//
//     [SerializeField] private string glowIntensityParam = "GlowIntensity";
//     [SerializeField] private string glowColorParam = "GlowColor";
//
//     [SerializeField] private float strongIntensity = 2f;
//     [SerializeField] private float weakIntensity = 0.4f;
//
//     [ColorUsage(true, true)]
//     [SerializeField] public Color strongColor = new Color(0.1f, 2f, 2f, 1f);
//
//     [ColorUsage(true, true)]
//     [SerializeField] public Color weakColor = new Color(0.05f, 0.3f, 0.3f, 1f);
//
//     private Material _mat;
//
//     public void SetMaterial(Material mat)
//     {
//         _mat = new Material(mat); // instancja per segment (dobrze!)
//         img.material = _mat;
//     }
//
//     public void SetSprite(Sprite sprite)
//     {
//         img.sprite = sprite;
//     }
//
//     public void SetStrong()
//     {
//         _mat.SetFloat(glowIntensityParam, strongIntensity);
//         _mat.SetColor(glowColorParam, strongColor);
//     }
//
//     public void SetWeak()
//     {
//         _mat.SetFloat(glowIntensityParam, weakIntensity);
//         _mat.SetColor(glowColorParam, weakColor);
//     }
//
//     public void SetOff()
//     {
//         _mat.SetFloat(glowIntensityParam, 0f);
//     }
// }