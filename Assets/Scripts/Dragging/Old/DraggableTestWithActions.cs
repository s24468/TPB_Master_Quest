// using UnityEngine;
// using System.Collections;
// using Cards;
//
// public class DraggableTestWithActions : MonoBehaviour
// {
//     public bool UsePointerDisplacement = true;
//
//     // PRIVATE FIELDS
//     private DraggingActionsTest da;
//
//     // a flag to know if we are currently dragging this GameObject
//     private bool dragging = false;
//
//     // distance from the center of this Game Object to the point where we clicked to start dragging 
//     private Vector3 pointerDisplacement = Vector3.zero;
//
//     // distance from camera to mouse on Z axis 
//     private float zDisplacement;
//
//     // MONOBEHAVIOUR METHODS
//     void Awake()
//     {
//         da = GetComponent<DraggingActionsTest>();
//     }
//
//     void OnMouseDown()
//     {
//
//         if (da.CanDrag)
//         {
//             dragging = true;
//             HoverPreview.PreviewsAllowed = false;
//             da.OnStartDrag();
//             zDisplacement = -Camera.main.transform.position.z + transform.position.z;
//             if (UsePointerDisplacement)
//                 pointerDisplacement = -transform.position + MouseInWorldCoords();
//             else
//                 pointerDisplacement = Vector3.zero;
//         }
//     }
//
//     void Update()
//     {
//         if (dragging)
//         {
//             Vector3 mousePos = MouseInWorldCoords();
//             da.OnDraggingInUpdate();
//             //Debug.Log(mousePos);
//             transform.position = new Vector3(mousePos.x - pointerDisplacement.x, mousePos.y - pointerDisplacement.y,
//                 transform.position.z);
//         }
//     }
//
//     void OnMouseUp()
//     {
//         if (dragging)
//         {
//             dragging = false;
//             HoverPreview.PreviewsAllowed = true;
//
//             da.OnEndDrag();
//         }
//     }
//
//     // returns mouse position in World coordinates for our GameObject to follow. 
//     private Vector3 MouseInWorldCoords()
//     {
//         var screenMousePos = Input.mousePosition;
//         //Debug.Log(screenMousePos);
//         screenMousePos.z = zDisplacement;
//         return Camera.main.ScreenToWorldPoint(screenMousePos);
//     }
// }