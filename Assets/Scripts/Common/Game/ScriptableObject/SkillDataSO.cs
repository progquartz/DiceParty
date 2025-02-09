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
// 타겟 데이터 받아온 다음에, 여기에 기반으로 하는 Effect를 상속받아서 하는 것으로.
// 타겟을 잡는 부분은, SkillDataSO에서 받아온 다음에, 이를 기반으로 작동.
// 효과 / 대상효과를 추가한 다음에 SkillDataSO로 만들 예정.

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    // 스킬 설명
    [Header("스킬 이름 / 설명")]
    public string skillName;

    [Header("스킬 담당 캐릭터")]
    public CharacterType CharacterType;

    [TextArea] public string skillLore;
    public int skillUseCount; // 0일 경우 무제한, 1일 경우 1회 사용, 그 이상의 경우 여러 회 사용.

    [Header("이 스킬이 가지는 여러 (이펙트 - 타겟팅) 조합")]
    public List<SkillEffectData> skillEffects;

    [Header("다이스 조건 설명")]
    public List<DiceRequirementData> diceRequirements;
}
