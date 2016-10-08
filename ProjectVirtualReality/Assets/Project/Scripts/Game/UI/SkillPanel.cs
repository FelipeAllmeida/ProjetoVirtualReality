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
    [SerializeField] private Button _buttonSummonPrefab;
    [SerializeField] private Button _buttonBlockPrefab;
    [SerializeField] private Button _buttonTrapPrefab;

    public void AInitialize()
    {
        InitializeButtons();
    }

    private void InitializeButtons()
    {
        _buttonSummonPrefab.onClick.AddListener(delegate
        {
            if (onClickPlayerSkillButton != null) onClickPlayerSkillButton(PlayerSkills.SkillType.SUMMON);
        });
    }
}
