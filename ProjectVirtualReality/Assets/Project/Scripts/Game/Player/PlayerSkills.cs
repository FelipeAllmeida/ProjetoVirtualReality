using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkills : MonoBehaviour 
{
    public enum SkillType
    {
        SUMMON,
        BLOCK,
        TRAPS
    }

    private List<SkillType> _listEnabledSkills;

    public void AInitialize()
    {
        InitializeEnableSkills();
    }

    private void InitializeEnableSkills()
    {
        _listEnabledSkills = new List<SkillType>();
    }

    private void AddEnabledSkill(SkillType p_skill)
    {
        if (_listEnabledSkills.Contains(p_skill) == true)
        {
            Debug.Log("Player already have skill " + p_skill.ToString());
            return;
        }

        _listEnabledSkills.Add(p_skill);
    }
}
