using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor
{
    public void UseSkill(SkillDataSO skillData)
    {
        if (skillData == null) return;

        // 스킬에 등록된 여러 (이펙트 - 타겟팅) 조합을 순회
        foreach (var effectData in skillData.skillEffects)
        {
            // 팩토리에서 이펙트와 타겟팅 로직 생성.
            BaseEffect effect = EffectFactory.CreateEffect(effectData.effectClassName);
            if (effect == null)
            {
                Debug.LogWarning($"Effect 생성 실패: {effectData.effectClassName}");
                continue;
            }

            BaseTargetOption targetOption = TargetOptionFactory.CreateTargetOption(effectData.targetClassName);
            if (targetOption == null)
            {
                Debug.LogWarning($"TargetOption 생성 실패: {effectData.targetClassName}");
                continue;
            }

            // 타겟 추출
            List<BaseTarget> targets = targetOption.GetTarget();

            // 이펙트 적용
            effect.Effect(targets, effectData.strength);
        }
    }
}
