using UnityEngine;
using System;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    #region Events
    public Action<Vector3> onMouseLeftClick;
    public Action<Vector3> onMouseRightClick;
    public Action<Vector3> onMouseMiddleClick;
    #endregion
    public void AInitialize()
    {
        
    }

    public void AUpdate()
    {
        HandlerPlayerInputs();
    }

    private void HandlerPlayerInputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 __mousePositionWorld = RaycastClickPositionToWorldPosition();
            Debug.Log(__mousePositionWorld);
            if (onMouseLeftClick != null) onMouseLeftClick(__mousePositionWorld);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 __mousePositionWorld = RaycastClickPositionToWorldPosition();
            if (onMouseRightClick != null) onMouseRightClick(__mousePositionWorld);
        }
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 __mousePositionWorld = RaycastClickPositionToWorldPosition();
            if (onMouseMiddleClick != null) onMouseMiddleClick(__mousePositionWorld);
        }        
    }

    private Vector3 RaycastClickPositionToWorldPosition()
    {
        Vector3 __clickWorldPosition = Vector3.zero;
        RaycastHit __raycastHit;
        Ray __ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(__ray, out __raycastHit))
        {
            __clickWorldPosition = __raycastHit.point;
        }
        return __clickWorldPosition;
    }
}
