using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
    private InputManager _inputManager;
    private PlayerSkills _playerSkills;

	public void AInitialize() 
    {
        InitializePlayer();
	}

    private void InitializePlayer()
    {
        InitializeInputManager();
        InitializePlayerSkills();
    }

    private void InitializeInputManager()
    {
        _inputManager = new InputManager();
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
