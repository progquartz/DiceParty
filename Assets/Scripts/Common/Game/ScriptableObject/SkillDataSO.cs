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

// 타겟 정보를 받아와 실행하고, 여기에 정의되어 있는 Effect를 상속받아서 하는 방식임.
// 타겟팅 하는 부분은, SkillDataSO에서 받아와 실행하고, 이를 순차적으로 작동.
// 효과 / 부효과들은 추가적인 정보는 SkillDataSO에 모두 포함.

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "Scriptable Objects/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    // 스킬 정보
    [Header("스킬 이름 / 설명")]
    public string skillName;

    [Header("스킬 사용 캐릭터")]
    public CharacterType CharacterType;

    [TextArea] public string skillLore;
    public int skillUseCount; // 0인 경우 무제한, 1인 경우 1회 사용, 그 이상인 경우 해당 회 사용.

    [Header("이 스킬이 가지는 효과 (이펙트 - 타겟팅) 정보")]
    public List<SkillEffectData> skillEffects;

    [Header("다이스 조건 정보")]
    public List<DiceRequirementData> diceRequirements;
}
