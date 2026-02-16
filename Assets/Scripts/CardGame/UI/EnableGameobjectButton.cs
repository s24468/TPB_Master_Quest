using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnableGameobjectButton : MonoBehaviour
{
    [SerializeField] private GameObject targetObject;

    public void OnButtonClick()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(!targetObject.activeSelf);
        }
    }
}