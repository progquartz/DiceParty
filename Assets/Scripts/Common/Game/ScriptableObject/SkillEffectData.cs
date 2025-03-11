using UnityEngine;


/// <summary>
/// 나머지의 경우...
/// 0 = 단순 계산 유형
/// 2 = 버프 형태, 버프 효과가 적용되는 모습이 보여야 함.
/// </summary>
[System.Serializable]
public enum EffectKey
{
    None = 0,
    DamageEffect = 10,
    DebuffDamageEffect = 20,
    ArmourEffect = 30,
    HealEffect = 40,
    CleanseEffect = 50,
    PoisonEffect = 12,
    FireEffect = 22,
    StunEffect = 32,
    StrengthEffect = 42,
    WeakenEffect = 52,
    AdditionalDiceEffect = 62,
    ConfuseEffect = 72,
    FortifyEffect = 82,
    PassionEffect = 92,
    RegenEffect = 102,
    TauntEffect = 112,
    ThornEffect = 122,
    WitherEffect = 132,
    ImmuneEffect = 142,
    

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
    public EffectKey effectClassName;

    [Header("스킬 강도(혹은 회복량, etc.)")]
    public int strength1;

    [Header("스킬 강도2(부가 수치))")]
    public int strength2;
}
