using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UITank : MonoBehaviour {

	public Image gunReloadStateImage;
	public Image turretRotationImage;
	public Image bodyRotationImage;
	public Image compassImage;
	public GameObject tankRoundSpritePrefab;
	public GameObject panel;
	public Action<GameStateTank.hudValues> onHudUpdateValues;
	public Camera mainCamera;
	private int _currentAmmo;
	private List<GameObject> _currentAmmoGOs;
	


	public void AInitialize () {
	
		onHudUpdateValues += HandlerHudUpdate;
	_currentAmmoGOs = new List<GameObject>();

	}
	

	void HandlerHudUpdate(GameStateTank.hudValues value)
	{

		gunReloadStateImage.fillAmount = value.reloadTime;
		#if UNITY_EDITOR
			bodyRotationImage.transform.localEulerAngles = new Vector3(bodyRotationImage.transform.localEulerAngles.x,bodyRotationImage.transform.localEulerAngles.y, -value.rotation +mainCamera.transform.parent.transform.parent.eulerAngles.y );
			compassImage.transform.localEulerAngles = new Vector3(bodyRotationImage.transform.localEulerAngles.x,bodyRotationImage.transform.localEulerAngles.y, mainCamera.transform.parent.transform.parent.eulerAngles.y );
		#else
			bodyRotationImage.transform.localEulerAngles = new Vector3(bodyRotationImage.transform.localEulerAngles.x,bodyRotationImage.transform.localEulerAngles.y, -value.rotation +mainCamera.transform.eulerAngles.y  );
			compassImage.transform.localEulerAngles = new Vector3(bodyRotationImage.transform.localEulerAngles.x,bodyRotationImage.transform.localEulerAngles.y, mainCamera.transform.eulerAngles.y  );
		#endif
		turretRotationImage.transform.localEulerAngles = new Vector3(turretRotationImage.transform.localEulerAngles.x,turretRotationImage.transform.localEulerAngles.y,-value.turretRotation);
		
		bodyRotationImage.color = new Color(value.health,1-value.health,0.1f);
		turretRotationImage.color = new Color(value.health,1-value.health,0.1f);
		if (value.ammo != _currentAmmo)
		{
			foreach (GameObject _goAmmo in _currentAmmoGOs)
				Destroy(_goAmmo);

			_currentAmmoGOs = new List<GameObject>();
		

			for (int i=0; i<value.ammo; i++)
			{

				GameObject __bulletSpirte = Instantiate(tankRoundSpritePrefab,panel.transform) as GameObject;
				__bulletSpirte.transform.localScale = new Vector3(1,1,1);
				Vector2 imageSizes = new Vector2( 4,20);
				__bulletSpirte.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,50+((imageSizes.x+2) * (i- ((int)i/24)*24)   ) ,imageSizes.x);
				__bulletSpirte.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top,5 +(((int)i/24)*imageSizes.y) ,imageSizes.y);
				_currentAmmoGOs.Add(__bulletSpirte);
			}
			_currentAmmo = value.ammo;
		}
		
	}
}
