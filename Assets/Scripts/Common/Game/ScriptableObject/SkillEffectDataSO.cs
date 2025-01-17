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
    [Header("대상 타겟팅 클래스")]
    public TargetOptionClassName targetClassName;

    [Header("주는 효과")]
    public EffectClassName effectClassName;

    [Header("스킬 강도(혹은 회복량, etc.)")]
    public int strength1;

    [Header("스킬 강도2(부가 수치))")]
    public int strength2;
}
