using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DropTarget : MonoBehaviour
{
    // To pozwala łatwo odczytać "co to za target" (TableVisual / ManaPoolVisual / cokolwiek)
    public IDropTarget Target { get; private set; }

    private void Awake()
    {
        // IDropTarget może być na tym samym obiekcie lub w parent (np. TableVisual na parent)
        Target = GetComponent<IDropTarget>() ?? GetComponentInParent<IDropTarget>();
        if (Target == null)
        {
            Debug.LogError($"[DropTarget] No IDropTarget found on {name} or parents.");
        }
    }
}