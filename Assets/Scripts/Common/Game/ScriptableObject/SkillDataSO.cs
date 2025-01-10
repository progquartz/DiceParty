using UnityEngine;

public enum SkillType
{
    Warrior = 0,
    Rogue = 1,
    Magician = 2,
    Preist = 3,
    All = 4
}

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    // 스킬 설명
    [Header("스킬 이름")]
    public string SkillName;
    [Header("스킬 설명")]
    public string SkillLore;

    // 스킬 담당
    [Header("스킬 담당")]
    public SkillType SkillType;


    [Header("스킬 조건 문자열")]
    public string diceNumLore;
    [Header("실제 다이스 문자열")]
    public int[] diceNum;
}
