using UnityEngine;

[RequireComponent(typeof(FrameGlowHighlighter))]
public class GlowOnHover : MonoBehaviour
{
    private FrameGlowHighlighter _glow;

    private void Awake() => _glow = GetComponent<FrameGlowHighlighter>();

    private void OnMouseEnter() => _glow.Show();
    private void OnMouseExit() => _glow.Hide();
}