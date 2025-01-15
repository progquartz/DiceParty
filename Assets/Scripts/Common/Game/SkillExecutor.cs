using System.Collections.Generic;
using UnityEngine;

public class SkillExecutor
{
    public void UseSkill(SkillDataSO skillData)
    {
        if (skillData == null) return;

        // ��ų�� ��ϵ� ���� (����Ʈ - Ÿ����) ������ ��ȸ
        foreach (var effectData in skillData.skillEffects)
        {
            // ���丮���� ����Ʈ�� Ÿ���� ���� ����.
            BaseEffect effect = EffectFactory.CreateEffect(effectData.effectClassName);
            if (effect == null)
            {
                Debug.LogWarning($"Effect ���� ����: {effectData.effectClassName}");
                continue;
            }

            BaseTargetOption targetOption = TargetOptionFactory.CreateTargetOption(effectData.targetClassName);
            if (targetOption == null)
            {
                Debug.LogWarning($"TargetOption ���� ����: {effectData.targetClassName}");
                continue;
            }

            // Ÿ�� ����
            List<BaseTarget> targets = targetOption.GetTarget();

            // ����Ʈ ����
            effect.Effect(targets, effectData.strength);
        }
    }
}
