using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    DamageEffect,
    HealEffect,

}

public enum TargetType
{
    TargetTargeted,
    TargetMostHPEnemy,
    TargetAllEnemy,
    TargetRandomEnemy,
    TargetAllPlayer
}
// Ÿ�� ������ �޾ƿ� ������, ���⿡ ������� �ϴ� Effect�� ��ӹ޾Ƽ� �ϴ� ������.
// Ÿ���� ��� �κ���, SkillDataSO���� �޾ƿ� ������, �̸� ������� �۵�.
// ȿ�� / ���ȿ���� �߰��� ������ SkillDataSO�� ���� ����.

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    // ��ų ����
    [Header("��ų �̸� / ����")]
    public string skillName;

    [Header("��ų ��� ĳ����")]
    public CharacterType CharacterType;

    [TextArea] public string skillLore;
    public int skillUseCount; // 0�� ��� ������, 1�� ��� 1ȸ ���, �� �̻��� ��� ���� ȸ ���.

    [Header("�� ��ų�� ������ ���� (����Ʈ - Ÿ����) ����")]
    public List<SkillEffectData> skillEffects;

    [Header("���̽� ���� ����")]
    public List<DiceRequirementData> diceRequirements;
}
