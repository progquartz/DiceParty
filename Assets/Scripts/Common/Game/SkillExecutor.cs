using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor
{
    public void UseSkill(SkillDataSO skillData, BaseTarget caller)
    {
        if (skillData == null) return;

        // ��ų�� ��ϵ� ���� (����Ʈ - Ÿ����) ������ ��ȸ
        foreach (var effectData in skillData.skillEffects)
        {
            ExecuteEffect(effectData, caller);
        }
    }


    public void UseSkill(SkillEffectData skillData, BaseTarget caller)
    {
        ExecuteEffect(skillData, caller);
    }

    public void UseSkill(SkillEffectData skillData)
    {
        ExecuteEffect(skillData);
    }

    public void UseSkill(List<SkillEffectData> skillData, BaseTarget caller)
    {
        if (skillData == null) return;

        foreach(var effData in skillData)
        {
            ExecuteEffect(effData, caller);
        }
    }

    private void ExecuteEffect(SkillEffectData skillData, BaseTarget caller = null)
    {
        if (skillData == null) return;

        // ���丮���� ����Ʈ�� Ÿ���� ���� ����.
        BaseEffect effect = EffectFactory.CreateEffect(skillData.effectClassName);
        if (effect == null)
        {
            Debug.LogWarning($"Effect ���� ����: {skillData.effectClassName}");
            return;
        }

        BaseTargetOption targetOption = TargetOptionFactory.CreateTargetOption(skillData.targetClassName);
        if (targetOption == null)
        {
            Debug.LogWarning($"TargetOption ���� ����: {skillData.targetClassName}");
            return;
        }

        // Ÿ�� ����
        List<BaseTarget> targets = targetOption.GetTarget(caller);

        // ����Ʈ ����
        effect.Effect(targets, skillData.strength1, skillData.strength2);
    }
}
