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
    [Header("��ų �̸�")]
    public string SkillName;
    [Header("��ų ����")]
    public string SkillLore;

    // ��ų ���
    [Header("��ų ���")]
    public SkillOwner SkillOwner;

    // ��ų ������ ���� ����
    [Header("��ų ����")]
    public List<SkillType> SkillType;
    public List<int> SkillStrength;


    [Header("��ų Ÿ��")]
    public List<TargetType> TargetOption;

    [Header("��ų ���� ���ڿ�")]
    public string diceNumLore;
    [Header("���� ���̽� ���ڿ�")]
    public int[] diceNum;
}
