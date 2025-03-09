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
    [Header("��� Ÿ���� Ŭ����")]
    public TargetOptionClassName targetClassName;

    [Header("�ִ� ȿ��")]
    public EffectClassName effectClassName;

    [Header("��ų ����(Ȥ�� ȸ����, etc.)")]
    public int strength1;

    [Header("��ų ����2(�ΰ� ��ġ))")]
    public int strength2;
}
