using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerSkills : MonoBehaviour 
{

	public Action<DataPacketServer> onDestroy;
	public Action<DataPacketServer> onCreate;
	
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
		_summonManager.onDestroy += onDestroy;
		_summonManager.onCreate += onCreate;
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

    public void UseSkill(int p_serial,SkillType p_skillType, object p_skillName, Vector3 p_position)
    {
		
        if (_listEnabledSkills.Contains(p_skillType) == true)
        {
            Vector3 __summonPosition = new Vector3(p_position.x, 1f, p_position.z);
            switch (p_skillType)
            {
                case SkillType.SUMMON:
                    SummonManager.SummonsType p_summonType = (SummonManager.SummonsType)p_skillName;
					_summonManager.Summon(p_serial,p_summonType, gameObject.transform, __summonPosition, new Quaternion(0f, 0f, 0f, 0f));
					
					
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
