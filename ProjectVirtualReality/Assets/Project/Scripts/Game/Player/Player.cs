using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    private CameraManager _cameraManager;

    private InputManager _inputManager;
    private PlayerSkills _playerSkills;

	public void AInitialize() 
    {
        InitializeCameraManager();
        InitializePlayer();
	}

    private void InitializeCameraManager()
    {
        _cameraManager = new CameraManager();
        _cameraManager.AInitialize();
    }

    private void InitializePlayer()
    {
        InitializeInputManager();
        InitializePlayerSkills();
    }

    private void InitializeInputManager()
    {
        _inputManager = new InputManager();
        ListenInputManagerEvents();
    }

    private void ListenInputManagerEvents()
    {
        _inputManager.onPressArrowLeft += delegate
        {
            _cameraManager.MoveCameraLeft();
        };
        _inputManager.onPressArrowRight += delegate
        {
            _cameraManager.MoveCameraRight();
        };
        _inputManager.onPressArrowUp += delegate
        {
            _cameraManager.MoveCameraUp();
        };
        _inputManager.onPressArrowDown += delegate
        {
            _cameraManager.MoveCameraDown();
        };
    }

    private void InitializePlayerSkills()
    {
        _playerSkills = gameObject.GetComponent<PlayerSkills>();
        _playerSkills.AInitialize();
        _playerSkills.AddEnabledSkill(PlayerSkills.SkillType.SUMMON);
    }

    public void AUpdate()
    {
       _inputManager.AUpdate();
    }

    public void HandleSkillClick(PlayerSkills.SkillType p_skillType)
    {
        _inputManager.onMouseLeftClick += delegate (Vector3 p_clickPosition)
        {
            HandleUseSummonSkill(p_skillType, p_clickPosition);
        };
    }

    private void HandleUseSummonSkill(PlayerSkills.SkillType p_skillType, Vector3 p_targetPosition)
    {
        _inputManager.onMouseLeftClick = null;
        _playerSkills.UseSkill(p_skillType, SummonManager.SummonsType.TURRET, p_targetPosition);
    }
}
