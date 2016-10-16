using UnityEngine;
using System;
using System.Collections;

public class InputManager : MonoBehaviour 
{
    #region Events
    public Action<Vector3> onMouseLeftClick;
    public Action<Vector3> onMouseRightClick;
    public Action<Vector3> onMouseMiddleClick;

    public event Action onPressArrowLeft;
    public event Action onPressArrowRight;
    public event Action onPressArrowUp;
    public event Action onPressArrowDown;
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
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (onPressArrowLeft != null) onPressArrowLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (onPressArrowRight != null) onPressArrowRight();
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (onPressArrowUp != null) onPressArrowUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            if (onPressArrowDown != null) onPressArrowDown();
        }  
          
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 __mousePositionWorld = RaycastClickPositionToWorldPosition();
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
