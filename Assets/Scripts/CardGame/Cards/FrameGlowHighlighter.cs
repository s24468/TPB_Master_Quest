using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FrameGlowHighlighter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image glowImage;          // FrameGlow -> Image
    [SerializeField] private bool startHidden = true;

    [Header("Visual")]
    [SerializeField] private Color glowColor;//= new Color(0.2f, 1f, 0.2f, 1f);
    [SerializeField] private float showAlpha = 1f;
    [SerializeField] private float fadeIn = 0.12f;
    [SerializeField] private float fadeOut = 0.12f;

    [Header("Pulse (optional)")]
    [SerializeField] private bool pulseWhenShown = false;
    [SerializeField] private float pulseMinAlpha = 0.55f;
    [SerializeField] private float pulseMaxAlpha = 1f;
    [SerializeField] private float pulseDuration = 0.6f;

    private Tween _tween;

    private void Awake()
    {
        if (glowImage == null)
        {
            Debug.LogError($"{nameof(FrameGlowHighlighter)}: Missing glowImage on {name}");
            enabled = false;
            return;
        }

        glowImage.raycastTarget = false; // ważne: glow nie blokuje myszy
        glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowImage.color.a);

        if (startHidden)
        {
            glowImage.gameObject.SetActive(true); // zawsze aktywny
            SetAlphaImmediate(0f);
        }

    }

    public void Show()
    {
        KillTween();
        glowImage.gameObject.SetActive(true);
        glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowImage.color.a);
    
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
        _tween = glowImage.DOFade(0f, fadeOut).SetEase(Ease.OutQuad);
    }

    public void SetColor(Color c)
    {
        glowColor = c;
        glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, glowImage.color.a);
    }

    public void SetAlphaImmediate(float a)
    {
        KillTween();
        var c = glowImage.color;
        glowImage.color = new Color(c.r, c.g, c.b, a);
        // NIE wyłączaj GameObject!
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
