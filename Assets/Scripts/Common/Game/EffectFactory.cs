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

        Logger.LogWarning($"{effectKey} �� �ش��ϴ� ����Ʈ �����ڸ� ã�� �� �����ϴ�.");
        return null;
    }
}
