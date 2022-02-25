using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillHandler 
{
    public bool HandleSkill(Skill skill, MonManager ally, MonManager enemy);
    public void HandleAnimation(HandleAnimations handleAnimation);
}