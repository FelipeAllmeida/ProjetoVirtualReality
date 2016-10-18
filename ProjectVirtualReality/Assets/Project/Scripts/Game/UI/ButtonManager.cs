using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ButtonManager : MonoBehaviour 
{
    #region Events
    public event Action<PlayerSkills.SkillType> onClickPlayerSkillButton;
    #endregion  

    [SerializeField] private PlayerSkills.SkillType _skillType;
    [SerializeField] private Button _button;
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _cooldown = 5f;
    private float _cooldownCounter = 5;


    public void InitializeButton()
    {
        _button.onClick.AddListener(delegate
        {
 
            if (_cooldownCounter >= _cooldown)
            {
                _cooldownCounter = 0;
                HandleButtonClick();
                _fillImage.fillAmount = 0f;
            }
        });
    }

    private void HandleButtonClick()
    {        
        if (onClickPlayerSkillButton != null) onClickPlayerSkillButton(_skillType);
    }

    public void AUpdate()
    {
        
        if (_fillImage.fillAmount < 1)
        {
            _cooldownCounter += Time.deltaTime;
            _fillImage.fillAmount = _cooldownCounter / _cooldown;
        }
    }


}
