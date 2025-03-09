using UnityEngine;


public enum EffectClassName
{
    DamageEffect,
    DebuffDamageEffect,
    ArmourEffect,
    HealEffect,
    PoisonEffect,
    FireEffect,
    StunEffect,
    StrengthEffect,
    WeakenEffect,
    AdditionalDiceEffect,
    CleanseEffect,
    ConfuseEffect,
    FortifyEffect,
    PassionEffect,
    TauntEffect,
    ThornEffect,
    WitherEffect

}

public enum TargetOptionClassName
{
    TargetAllEnemy,
    TargetRandomEnemy,
    TargetRandomCharacter,
    TargetAllCharacter,
    TargetAllEnemyDead,
    TargetRandomEnemyDead,
    TargetRandomCharacterDead,
    TargetAllCharacterDead,
    TargetSelf
}

[System.Serializable]
public class SkillEffectData
{
    [Header("대상 타겟팅 클래스")]
    public TargetOptionClassName targetClassName;

    [Header("주는 효과")]
    public EffectClassName effectClassName;

    [Header("스킬 강도(혹은 회복량, etc.)")]
    public int strength1;

    [Header("스킬 강도2(부가 수치))")]
    public int strength2;
}
