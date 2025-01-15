using UnityEngine;

[CreateAssetMenu(fileName = "SkillEffectDataSO", menuName = "Scriptable Objects/SkillEffectDataSO")]
public class SkillEffectDataSO : ScriptableObject
{
    [Header("이펙트 클래스 이름 (예: DamageEffect, HealEffect 등)")]
    public string effectClassName;

    [Header("스킬 강도(혹은 회복량, etc.)")]
    public int strength;

    [Header("타겟팅 클래스 이름 (예: TargetAllEnemy, TargetRandomEnemy 등)")]
    public string targetClassName;
}
