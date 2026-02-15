using Dragging;
using UnityEngine;

public class Targetable : MonoBehaviour
{
    [SerializeField] private FrameGlowHighlighter glow;

    public IAttackable Attackable { get; private set; }
    public FrameGlowHighlighter Glow => glow;

    private void Awake()
    {
        // Cache raz, nie w pętli
        Attackable = GetComponentInParent<IAttackable>();

        if (glow == null)
            glow = GetComponentInChildren<FrameGlowHighlighter>(true);
    }
}