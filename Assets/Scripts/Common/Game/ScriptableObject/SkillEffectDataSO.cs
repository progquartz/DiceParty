using UnityEngine;

[CreateAssetMenu(fileName = "SkillEffectDataSO", menuName = "Scriptable Objects/SkillEffectDataSO")]
public class SkillEffectDataSO : ScriptableObject
{
    [Header("����Ʈ Ŭ���� �̸� (��: DamageEffect, HealEffect ��)")]
    public string effectClassName;

    [Header("��ų ����(Ȥ�� ȸ����, etc.)")]
    public int strength;

    [Header("Ÿ���� Ŭ���� �̸� (��: TargetAllEnemy, TargetRandomEnemy ��)")]
    public string targetClassName;
}
