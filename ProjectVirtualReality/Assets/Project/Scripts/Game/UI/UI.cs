using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UI : MonoBehaviour 
{
    #region SerializableFields
    [Header("User Interface Members")]
    [SerializeField] private SkillPanel _skillPanel;
    #endregion

    #region Events
    public event Action<PlayerSkills.SkillType> onClickPlayerSkillButton; 
    #endregion
    public void AInitialize () 
    {
        ListenEvents();
        InitializeSkillPanel();
    }

    private void InitializeSkillPanel()
    {
        _skillPanel.AInitialize();
    }

    private void ListenEvents()
    {
        _skillPanel.onClickPlayerSkillButton += delegate (PlayerSkills.SkillType p_skillType)
        {
            if (onClickPlayerSkillButton != null) onClickPlayerSkillButton(p_skillType);
        };  
    }
	
	public void AUpdate () 
    {
        _skillPanel.AUpdate();
	}
}
