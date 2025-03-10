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
        { "DebuffDamageEffect",   () => new DebuffDamageEffect() },
        { "HealEffect",   () => new HealEffect() },   
        { "AdditionalDiceEffect",   () => new AdditionalDiceEffect() },   
        { "CleanseEffect",   () => new CleanseEffect() },   
        { "ConfuseEffect",   () => new ConfuseEffect() },   
        { "FireEffect",   () => new FireEffect() },   
        { "FortifyEffect",   () => new FortifyEffect() },   
        { "PassionEffect",   () => new PassionEffect() },   
        { "PoisonEffect",   () => new PoisonEffect() },   
        { "StrengthEffect",   () => new StrengthEffect() },   
        { "StunEffect",   () => new StunEffect() },   
        { "TauntEffect",   () => new TauntEffect() },   
        { "ThornEffect",   () => new ThornEffect() },   
        { "WeakenEffect",   () => new WeakenEffect() },   
        { "WitherEffect",   () => new WitherEffect() },
        { "RegenEffect",   () => new RegenEffect() },
        { "ImmuneEffect",   () => new ImmuneEffect() },
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
