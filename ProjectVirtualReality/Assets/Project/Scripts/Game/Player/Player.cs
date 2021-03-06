﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Player : MonoBehaviour 
{
    private CameraManager _cameraManager;

    private InputManager _inputManager;
    private PlayerSkills _playerSkills;
	public Action<DataPacketServer> onDestroy;
	public Action<DataPacketServer> onCreate;


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
		_playerSkills.onDestroy += onDestroy;
		_playerSkills.onCreate += onCreate;

	
        _playerSkills.AInitialize();
        _playerSkills.AddEnabledSkill(PlayerSkills.SkillType.SUMMON);
		
    }

    public void AUpdate()
    {
       _inputManager.AUpdate();
    }

    public void HandleSkillClick(int p_serialData, PlayerSkills.SkillType p_skillType)
    {
        _inputManager.onMouseLeftClick += delegate (Vector3 p_clickPosition)
        {
            HandleUseSummonSkill(p_serialData, p_skillType, p_clickPosition);
        };
    }

    private void HandleUseSummonSkill(int p_serialData, PlayerSkills.SkillType p_skillType, Vector3 p_targetPosition)
    {
        _inputManager.onMouseLeftClick = null;
        _playerSkills.UseSkill(p_serialData, p_skillType, SummonManager.SummonsType.TURRET, p_targetPosition);
    }
}
