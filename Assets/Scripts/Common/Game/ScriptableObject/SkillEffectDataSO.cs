using UnityEngine;


public enum EffectClassName
{
    DamageEffect,
    HealEffect,

}

public enum TargetOptionClassName
{
    TargetAllEnemy,
    TargetRandomEnemy,
}

[System.Serializable]
public class SkillEffectDataSO
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
