using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
//
// public class FrameGlowHighlighter : MonoBehaviour
// {
//     [Header("References")]
//     [SerializeField] private Image glowImage;          // FrameGlow -> Image
//     [SerializeField] private bool startHidden = true;
//
//     [Header("Visual")]
//     [SerializeField] private Color glowColor;//= new Color(0.2f, 1f, 0.2f, 1f);
//     [SerializeField] private float showAlpha = 1f;
//     [SerializeField] private float fadeIn = 0.12f;
//     [SerializeField] private float fadeOut = 0.12f;
//
//     [Header("Pulse (optional)")]
//     [SerializeField] private bool pulseWhenShown = false;
//     [SerializeField] private float pulseMinAlpha = 0.55f;
//     [SerializeField] private float pulseMaxAlpha = 1f;
//     [SerializeField] private float pulseDuration = 0.6f;
//
//     private Tween _tween;
//
//     private void Awake()
//     {
//         if (glowImage == null)
//         {
//             Debug.LogError($"{nameof(FrameGlowHighlighter)}: Missing glowImage on {name}");
//             enabled = false;
//             return;
//         }
//
//         glowImage.raycastTarget = false; // ważne: glow nie blokuje myszy
//         glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowImage.color.a);
//
//         if (startHidden)
//         {
//             glowImage.gameObject.SetActive(true); // zawsze aktywny
//             SetAlphaImmediate(0f);
//         }
//
//     }
//
//     public void Show()
//     {
//         KillTween();
//         glowImage.gameObject.SetActive(true);
//         glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowImage.color.a);
//     
//         if (pulseWhenShown)
//         {
//             SetAlphaImmediate(pulseMinAlpha);
//             _tween = glowImage.DOFade(pulseMaxAlpha, pulseDuration)
//                 .SetEase(Ease.InOutSine)
//                 .SetLoops(-1, LoopType.Yoyo);
//         }
//         else
//         {
//             _tween = glowImage.DOFade(showAlpha, fadeIn).SetEase(Ease.OutQuad);
//         }
//     }
//     public void Hide()
//     {
//         KillTween();
//         _tween = glowImage.DOFade(0f, fadeOut).SetEase(Ease.OutQuad);
//     }
//
//     public void SetColor(Color c)
//     {
//         glowColor = c;
//         glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowImage.color.a);
//     }
//
//     public void SetAlphaImmediate(float a)
//     {
//         KillTween();
//         var c = glowImage.color;
//         glowImage.color = new Color(c.r, c.g, c.b, a);
//         // NIE wyłączaj GameObject!
//     }
//
//     private void KillTween()
//     {
//         if (_tween != null && _tween.IsActive())
//             _tween.Kill();
//         _tween = null;
//     }
//
//     private void OnDestroy()
//     {
//         KillTween();
//     }
// }
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FrameGlowHighlighter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image glowImage;
    [SerializeField] private bool startHidden = true;

    [Header("Shader Params (ShaderGraph)")]
    [SerializeField] private string glowColorParam = "_GlowColor";       // jeśli w graphie Reference = GlowColor, Unity zwykle zrobi _GlowColor
    [SerializeField] private string glowIntensityParam = "_GlowIntensity"; // jw.

    [Header("Visual")]
    [SerializeField] private Color glowColor = Color.green; // HDR mile widziane (wartości > 1)
    [SerializeField] private float shownIntensity = 1f;     // ile ma świecić gdy włączone
    [SerializeField] private float hiddenIntensity = 0f;    // ile ma świecić gdy wyłączone
    [SerializeField] private float showAlpha = 1f;
    [SerializeField] private float fadeIn = 0.12f;
    [SerializeField] private float fadeOut = 0.12f;

    [Header("Pulse (optional)")]
    [SerializeField] private bool pulseWhenShown = false;
    [SerializeField] private float pulseMinAlpha = 0.55f;
    [SerializeField] private float pulseMaxAlpha = 1f;
    [SerializeField] private float pulseDuration = 0.6f;

    private Tween _tween;
    private Material _matInstance;

    private void Awake()
    {
        if (glowImage == null)
        {
            Debug.LogError($"{nameof(FrameGlowHighlighter)}: Missing glowImage on {name}");
            enabled = false;
            return;
        }

        glowImage.raycastTarget = false;

        // Bardzo ważne: instancja materiału, żeby nie zmieniać wszystkim naraz
        if (glowImage.material != null)
        {
            _matInstance = Instantiate(glowImage.material);
            glowImage.material = _matInstance;
        }

        ApplyShaderColor(glowColor);

        if (startHidden)
        {
            glowImage.gameObject.SetActive(true);
            SetAlphaImmediate(0f);
            SetIntensityImmediate(hiddenIntensity);
        }
        else
        {
            SetIntensityImmediate(shownIntensity);
        }
    }

    public void Show()
    {
        KillTween();
        glowImage.gameObject.SetActive(true);

        ApplyShaderColor(glowColor);
        SetIntensityImmediate(shownIntensity);

        if (pulseWhenShown)
        {
            SetAlphaImmediate(pulseMinAlpha);
            _tween = glowImage.DOFade(pulseMaxAlpha, pulseDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
        else
        {
            _tween = glowImage.DOFade(showAlpha, fadeIn).SetEase(Ease.OutQuad);
        }
    }

    public void Hide()
    {
        KillTween();

        // równolegle: gaś alpha + intensity
        Sequence s = DOTween.Sequence();
        s.Join(glowImage.DOFade(0f, fadeOut).SetEase(Ease.OutQuad));
        s.Join(DOTween.To(() => GetIntensity(), v => SetIntensity(v), hiddenIntensity, fadeOut).SetEase(Ease.OutQuad));

        _tween = s;
    }

    public void SetColor(Color c)
    {
        glowColor = c;
        ApplyShaderColor(glowColor);
    }

    public void SetAlphaImmediate(float a)
    {
        KillTween();
        var c = glowImage.color;
        glowImage.color = new Color(c.r, c.g, c.b, a);
    }

    public void SetIntensityImmediate(float v)
    {
        SetIntensity(v);
    }

    // ====== shader helpers ======

    private void ApplyShaderColor(Color c)
    {
        if (_matInstance == null) return;

        // Uwaga: w ShaderGraph "Reference" może być dokładnie "GlowColor" bez podkreślnika.
        // Jeśli nie działa, wstaw tu "GlowColor" zamiast "_GlowColor" (albo podejrzyj nazwę w material inspector/debug).
        if (_matInstance.HasProperty(glowColorParam))
            _matInstance.SetColor(glowColorParam, c);
        else if (_matInstance.HasProperty("GlowColor"))
            _matInstance.SetColor("GlowColor", c);
    }

    private float GetIntensity()
    {
        if (_matInstance == null) return 0f;

        if (_matInstance.HasProperty(glowIntensityParam))
            return _matInstance.GetFloat(glowIntensityParam);
        if (_matInstance.HasProperty("GlowIntensity"))
            return _matInstance.GetFloat("GlowIntensity");

        return 0f;
    }

    private void SetIntensity(float v)
    {
        if (_matInstance == null) return;

        if (_matInstance.HasProperty(glowIntensityParam))
            _matInstance.SetFloat(glowIntensityParam, v);
        else if (_matInstance.HasProperty("GlowIntensity"))
            _matInstance.SetFloat("GlowIntensity", v);
    }

    private void KillTween()
    {
        if (_tween != null && _tween.IsActive())
            _tween.Kill();
        _tween = null;
    }

    private void OnDestroy()
    {
        KillTween();
    }
}