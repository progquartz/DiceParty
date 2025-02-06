using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor
{
    public void UseSkill(SkillDataSO skillData, BaseTarget caller)
    {
        if (skillData == null) return;

        // 스킬에 등록된 여러 (이펙트 - 타겟팅) 조합을 순회
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

        // 팩토리에서 이펙트와 타겟팅 로직 생성.
        BaseEffect effect = EffectFactory.CreateEffect(skillData.effectClassName);
        if (effect == null)
        {
            Debug.LogWarning($"Effect 생성 실패: {skillData.effectClassName}");
            return;
        }

        BaseTargetOption targetOption = TargetOptionFactory.CreateTargetOption(skillData.targetClassName);
        if (targetOption == null)
        {
            Debug.LogWarning($"TargetOption 생성 실패: {skillData.targetClassName}");
            return;
        }

        // 타겟 추출
        List<BaseTarget> targets = targetOption.GetTarget(caller);

        // 이펙트 적용
        effect.Effect(targets, skillData.strength1, skillData.strength2);
    }
}
