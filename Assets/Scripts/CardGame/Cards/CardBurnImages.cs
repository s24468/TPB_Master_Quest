// using System;
// using DG.Tweening;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class CardBurnTwoImages : MonoBehaviour
// {
//     [Header("Assign in Inspector")]
//     [SerializeField] private CanvasGroup canvasGroup;   // na CardPanel
//     [SerializeField] private Image cardGraphic;         // CardGraphic (Image)
//     [SerializeField] private Image cardBody;            // CardBody (Image) - jeśli to Image
//
//     [Header("Shader")]
//     [SerializeField] private string dissolveParam = "_DissolveAmount";
//
//     private Material graphicMatInstance;
//     private Material bodyMatInstance;
//
//     private void Awake()
//     {
//         if (canvasGroup == null)
//             canvasGroup = GetComponentInChildren<CanvasGroup>(true);
//
//         // Jeśli nie podepniesz ręcznie, spróbuj znaleźć (ale lepiej podepnij w Inspectorze)
//         if (cardGraphic == null)
//             cardGraphic = GetComponentInChildren<Image>(true);
//
//         // Instancje materiałów (ważne!)
//         if (cardGraphic != null && cardGraphic.material != null)
//         {
//             graphicMatInstance = Instantiate(cardGraphic.material);
//             cardGraphic.material = graphicMatInstance;
//         }
//
//         if (cardBody != null && cardBody.material != null)
//         {
//             bodyMatInstance = Instantiate(cardBody.material);
//             cardBody.material = bodyMatInstance;
//         }
//     }
//
//     public void Burn(float duration = 0.6f, float fadeDuration = 0.35f, Action onComplete = null)
//     {
//         // start: widoczna
//         if (canvasGroup != null) canvasGroup.alpha = 1f;
//
//         // u Ciebie: 1 = widać, 0 = znika
//         if (graphicMatInstance != null) graphicMatInstance.SetFloat(dissolveParam, 0f);
//         if (bodyMatInstance != null) bodyMatInstance.SetFloat(dissolveParam, 0f);
//
//         Sequence s = DOTween.Sequence();
//
//         // dissolve równolegle (0 -> 1)
//         if (graphicMatInstance != null)
//         {
//             s.Join(DOTween.To(
//                 () => graphicMatInstance.GetFloat(dissolveParam),
//                 v => graphicMatInstance.SetFloat(dissolveParam, v),
//                 1f,
//                 duration
//             ));
//         }
//
//         if (bodyMatInstance != null)
//         {
//             s.Join(DOTween.To(
//                 () => bodyMatInstance.GetFloat(dissolveParam),
//                 v => bodyMatInstance.SetFloat(dissolveParam, v),
//                 1f,
//                 duration
//             ));
//         }
//
//
//         // fade tekstów i reszty UI przez CanvasGroup (krótszy, żeby szybciej znikały)
//         if (canvasGroup != null)
//             s.Join(canvasGroup.DOFade(0f, duration/2).SetEase(Ease.InQuad));
//
//         s.OnComplete(() => onComplete?.Invoke());
//     }
// }
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardBurnImages : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] private CanvasGroup canvasGroup;   // na CardPanel (żeby schować TMP itd.)
    [SerializeField] private Image cardGraphic;         // CardGraphic (Image)
    [SerializeField] private Image cardBody;            // CardBody (Image)
    [SerializeField] private Image cardFrame;           // CardFrame (Image)
    [SerializeField] private Image frame;               // Frame (Image)

    [Header("Shader")]
    [SerializeField] private string dissolveParam = "_DissolveAmount";

    private Material graphicMatInstance;
    private Material bodyMatInstance;
    private Material cardFrameMatInstance;
    private Material frameMatInstance;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponentInChildren<CanvasGroup>(true);

        // Instancje materiałów (ważne żeby nie rozpuszczać wszystkich kart naraz)
        graphicMatInstance = MakeMaterialInstance(cardGraphic);
        bodyMatInstance = MakeMaterialInstance(cardBody);
        cardFrameMatInstance = MakeMaterialInstance(cardFrame);
        frameMatInstance = MakeMaterialInstance(frame);
    }

    private Material MakeMaterialInstance(Image img)
    {
        if (img == null) return null;
        if (img.material == null) return null;

        var inst = Instantiate(img.material);
        img.material = inst;
        return inst;
    }

    public void Burn(float duration = 0.6f, float textFadeDuration = -1f, Action onComplete = null)
    {
        if (canvasGroup != null) canvasGroup.alpha = 1f;

        // u Ciebie: 0 = widać, 1 = znika
        SetDissolve(graphicMatInstance, 0f);
        SetDissolve(bodyMatInstance, 0f);
        SetDissolve(cardFrameMatInstance, 0f);
        SetDissolve(frameMatInstance, 0f);

        if (textFadeDuration < 0f) textFadeDuration = duration * 0.5f;

        Sequence s = DOTween.Sequence();

        // dissolve równolegle (0 -> 1) na wszystkich materiałach
        JoinDissolve(s, graphicMatInstance, duration);
        JoinDissolve(s, bodyMatInstance, duration);
        JoinDissolve(s, cardFrameMatInstance, duration);
        JoinDissolve(s, frameMatInstance, duration);

        // TMP i reszta przez CanvasGroup (też startuje w tym samym momencie)
        if (canvasGroup != null)
            s.Join(canvasGroup.DOFade(0f, textFadeDuration).SetEase(Ease.InQuad));

        s.OnComplete(() => onComplete?.Invoke());
    }

    private void SetDissolve(Material mat, float value)
    {
        if (mat == null) return;
        mat.SetFloat(dissolveParam, value);
    }

    private void JoinDissolve(Sequence s, Material mat, float duration)
    {
        if (mat == null) return;

        s.Join(DOTween.To(
            () => mat.GetFloat(dissolveParam),
            v => mat.SetFloat(dissolveParam, v),
            1f,
            duration
        ));
    }
}