using UnityEngine;


/// <summary>
/// 이펙트의 종류...
/// 0 = 단순 효과 없음
/// 2 = 지속 효과, 지속 효과가 적용되는 대상의 정보를 줌.
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
    [Header("효과 타겟팅 클래스")]
    public TargetOptionClassName targetClassName;

    [Header("주는 효과")]
    public EffectKey effectClassName;

    [Header("스킬 수치(혹은 회복량, etc.)")]
    public int strength1;

    [Header("스킬 수치2(이격 거리))")]
    public int strength2;
}
