using System.Collections.Generic;
using UnityEngine;

public enum SkillOwner
{
    Warrior = 0,
    Rogue = 1,
    Magician = 2,
    Preist = 3,
    All = 4
}

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
    [TextArea] public string skillLore;

    [Header("�� ��ų�� ������ ���� (����Ʈ - Ÿ����) ����")]
    public List<SkillEffectData> skillEffects;

    [Header("���̽� ���� ����")]
    public List<DiceRequirementData> diceRequirements;
}
