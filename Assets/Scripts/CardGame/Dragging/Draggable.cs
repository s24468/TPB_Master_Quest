using UnityEngine;
using System.Collections;
using Cards;
using DG.Tweening;

public class Draggable : MonoBehaviour
{
    private bool _dragging = false;
    private Vector3 _pointerDisplacement;
    private float _zDisplacement;
    private DraggingActions _da;
    private static Draggable _draggingThis;

    public static Draggable DraggingThis
    {
        get { return _draggingThis; }
    }

    void Awake()
    {
        _da = GetComponent<DraggingActions>();
    }

    void OnMouseDown()
    {
        if (_da != null && _da.CanDrag)// 
        {
            _dragging = true;
            // when we are dragging something, all previews should be off
            HoverPreview.PreviewsAllowed = false;
            _draggingThis = this;
            _da.OnStartDrag();
            _zDisplacement = -Camera.main.transform.position.z + transform.position.z;
            _pointerDisplacement = -transform.position + MouseInWorldCoords();
        }
    }

    void Update()
    {
        if (_dragging)
        {
            Vector3 mousePos = MouseInWorldCoords();
            // Debug.Log(mousePos);
            transform.position = new Vector3(mousePos.x - _pointerDisplacement.x, mousePos.y - _pointerDisplacement.y,
                transform.position.z);
            _da.OnDraggingInUpdate();
        }
    }

    void OnMouseUp()
    {
        if (_dragging)
        {
            _dragging = false;
            // turn all previews back on
            HoverPreview.PreviewsAllowed = true;
            _draggingThis = null;
            _da.OnEndDrag();
        }
    }

    // returns mouse position in World coordinates for our GameObject to follow. 
    private Vector3 MouseInWorldCoords()
    {
        var screenMousePos = Input.mousePosition;
        //Debug.Log(screenMousePos);
        screenMousePos.z = _zDisplacement;
        return Camera.main.ScreenToWorldPoint(screenMousePos);
    }
}