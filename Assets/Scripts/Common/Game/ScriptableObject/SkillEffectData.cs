using UnityEngine;


/// <summary>
/// �������� ���...
/// 0 = �ܼ� ��� ����
/// 2 = ���� ����, ���� ȿ���� ����Ǵ� ����� ������ ��.
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
    [Header("��� Ÿ���� Ŭ����")]
    public TargetOptionClassName targetClassName;

    [Header("�ִ� ȿ��")]
    public EffectKey effectClassName;

    [Header("��ų ����(Ȥ�� ȸ����, etc.)")]
    public int strength1;

    [Header("��ų ����2(�ΰ� ��ġ))")]
    public int strength2;
}
