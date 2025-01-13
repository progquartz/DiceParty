using UnityEngine;

public enum SkillType
{
    Warrior = 0,
    Rogue = 1,
    Magician = 2,
    Preist = 3,
    All = 4
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
    public SkillType SkillType;


    [Header("��ų ���� ���ڿ�")]
    public string diceNumLore;
    [Header("���� ���̽� ���ڿ�")]
    public int[] diceNum;
}
