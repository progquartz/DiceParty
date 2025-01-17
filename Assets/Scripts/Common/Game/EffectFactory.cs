using System;
using System.Collections.Generic;
using UnityEngine;

public static class EffectFactory
{
    // string(Ŭ���� �̸�) -> Func<BaseEffect> ������
    private static Dictionary<string, Func<BaseEffect>> effectRegistry
        = new Dictionary<string, Func<BaseEffect>>()
    {
        { "DamageEffect", () => new DamageEffect() },
        { "HealEffect",   () => new HealEffect() },   // ��: ġ�� ȿ�� �߰� ��
    };

    public static BaseEffect CreateEffect(EffectClassName effectKey)
    {
        if (effectRegistry.TryGetValue(effectKey.ToString(), out var constructor))
        {
            return constructor();
        }

        Logger.LogWarning($"{effectKey} �� �ش��ϴ� ����Ʈ �����ڸ� ã�� �� �����ϴ�.");
        return null;
    }
}
