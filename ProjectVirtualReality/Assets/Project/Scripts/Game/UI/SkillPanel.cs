using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class SkillPanel : MonoBehaviour
{
    #region Events
    public event Action<PlayerSkills.SkillType> onClickPlayerSkillButton;
    #endregion    

    [Header("Button Prefabs")]
    [SerializeField] private ButtonManager _buttonSummonPrefab;
    [SerializeField] private ButtonManager _buttonBlockPrefab;
    [SerializeField] private ButtonManager _buttonTrapPrefab;

    public void AInitialize()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        _buttonSummonPrefab.InitializeButton();
        _buttonSummonPrefab.onClickPlayerSkillButton += delegate (PlayerSkills.SkillType p_skillType)
        {
            if (onClickPlayerSkillButton != null)
                onClickPlayerSkillButton(p_skillType);
        };

    }

    public void AUpdate()
    {
        _buttonSummonPrefab.AUpdate();
    }
}
