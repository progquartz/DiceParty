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
// 타겟 데이터 받아온 다음에, 여기에 기반으로 하는 Effect를 상속받아서 하는 것으로.
// 타겟을 잡는 부분은, SkillDataSO에서 받아온 다음에, 이를 기반으로 작동.
// 효과 / 대상효과를 추가한 다음에 SkillDataSO로 만들 예정.

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
    public SkillOwner SkillOwner;

    // 스킬 유형과 강함 정도
    [Header("스킬 유형")]
    public List<SkillType> SkillType;
    public List<int> SkillStrength;


    [Header("스킬 타겟")]
    public List<TargetType> TargetOption;

    [Header("스킬 조건 문자열")]
    public string diceNumLore;
    [Header("실제 다이스 문자열")]
    public int[] diceNum;
}
