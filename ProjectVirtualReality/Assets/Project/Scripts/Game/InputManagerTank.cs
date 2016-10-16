using UnityEngine;
using System;
using System.Collections;

public class InputManagerTank : MonoBehaviour 
{
    #region Events
    public Action<Vector3> onMouseLeftClick;
    public Action<Vector3> onMouseRightClick;
    public Action<Vector3> onMouseMiddleClick;

	public Action<Vector2> onPressHorizontalAxis;
    public Action<Vector2> onPressVerticalAxis;

	public Action<float> onPressTurretRotate;
	public Action<float> onPressGunRotate;
	
	public Action onPressFire;

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

		if (Input.GetButtonDown("Fire"))
		{

			if(onPressFire != null) onPressFire();

		}

		float _xAxis = Input.GetAxis("Horizontal");
		float _zAxis = Input.GetAxis("Vertical");

        if (_xAxis!=0);
        {
			         
			if (onPressHorizontalAxis != null) onPressHorizontalAxis(new Vector2(-_xAxis,_xAxis));
        }
        if (_zAxis != 0)
        {
            if (onPressVerticalAxis != null) onPressVerticalAxis(new Vector2(_zAxis,_zAxis));
        }
       

		float _turretRotate = Input.GetAxis("TurretRotate");
		float _gunRotate = Input.GetAxis("GunRotate");

		if (_turretRotate!= 0)
		{
		
			if(onPressTurretRotate != null) onPressTurretRotate(_turretRotate);

		}
		if (_gunRotate != 0)
		{
			if(onPressGunRotate != null) onPressGunRotate(_gunRotate );
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
