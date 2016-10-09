using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
    private GameObject _cameraObject;

    public void AInitialize()
    {
        _cameraObject = GameObject.Find("Main Camera");
    }

    public void MoveCameraLeft()
    {
        _cameraObject.transform.Translate(Vector3.left);
    }

    public void MoveCameraRight()
    {
        _cameraObject.transform.Translate(Vector3.right);
    }

    public void MoveCameraUp()
    {
        _cameraObject.transform.Translate(Vector3.up);
    }

    public void MoveCameraDown()
    {
        _cameraObject.transform.Translate(Vector3.down);
    }

    public void MoveCameraFoward()
    {
        _cameraObject.transform.Translate(Vector3.forward);
    }

    public void MoveCameraBack()
    {
        _cameraObject.transform.Translate(Vector3.back);
    }
}
