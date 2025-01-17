using System;
using System.Collections.Generic;
using UnityEngine;

public static class EffectFactory
{
    // string(클래스 이름) -> Func<BaseEffect> 생성자
    private static Dictionary<string, Func<BaseEffect>> effectRegistry
        = new Dictionary<string, Func<BaseEffect>>()
    {
        { "DamageEffect", () => new DamageEffect() },
        { "HealEffect",   () => new HealEffect() },   // 예: 치유 효과 추가 시
    };

    public static BaseEffect CreateEffect(EffectClassName effectKey)
    {
        if (effectRegistry.TryGetValue(effectKey.ToString(), out var constructor))
        {
            return constructor();
        }

        Logger.LogWarning($"{effectKey} 에 해당하는 이펙트 생성자를 찾을 수 없습니다.");
        return null;
    }
}
