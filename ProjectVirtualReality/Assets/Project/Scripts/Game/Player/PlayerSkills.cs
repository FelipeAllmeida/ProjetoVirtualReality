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
    private SummonManager _summonManager;
    private List<SkillType> _listEnabledSkills;

    public void AInitialize()
    {
        InitializeEnableSkills();
        InitializeSkillsManager();
    }

    private void InitializeSkillsManager()
    {
        _summonManager = gameObject.GetComponent<SummonManager>();

        _summonManager.Initialize();
    }

    private void InitializeEnableSkills()
    {
        _listEnabledSkills = new List<SkillType>();
    }

    public void AddEnabledSkill(SkillType p_skill)
    {
        if (_listEnabledSkills.Contains(p_skill) == true)
        {
            Debug.Log("Player already have skill " + p_skill.ToString());
            return;
        }

        _listEnabledSkills.Add(p_skill);
    }

    public void UseSkill(SkillType p_skillType, object p_skillName, Vector3 p_position)
    {
        if (_listEnabledSkills.Contains(p_skillType) == true)
        {
            switch (p_skillType)
            {
                case SkillType.SUMMON:
                    SummonManager.SummonsType p_summonType = (SummonManager.SummonsType)p_skillName;
                    _summonManager.Summon(p_summonType, gameObject.transform, p_position, new Quaternion(0f, 0f, 0f, 0f));
                    break;
                case SkillType.BLOCK:
                    break;
                case SkillType.TRAPS:
                    break;
            }
        }
        else
        {
            return;
        }
    }
}
