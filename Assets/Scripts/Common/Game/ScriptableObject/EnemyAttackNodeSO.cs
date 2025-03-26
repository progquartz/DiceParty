using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyAttackNodeSO", menuName = "Scriptable Objects/EnemyAttackNodeSO")]
public class EnemyAttackNodeSO : ScriptableObject
{
    [Header("이 노드에서 사용할 공격 리스트(1개 이상 필요)")]
    [Header("공격명")]
    public string attackName;

    // 설명하기 모르겠다면...
    [Header("공격 시 하게 되는 행동")]
    public string attackText;

    [Header("스킬로 사용될 되는 이펙트")]
    public List<SkillEffectData> attackEffects;


    // 다음 노드 설정

    [Header("다음 노드 선택 방식의 종류")]
    public TransitionType transitionType;

    // 순차 이동
    [Tooltip("순차 이동일 때, 다음 노드를 지정")]
    public EnemyAttackNodeSO nextNode;


    [Header("확률 선택 분기 시 사용")]
    // 확률 선택 분기
    [Tooltip("선택이 확률 기반일때 확인")]
    public List<ProbabilityTransition> probabilityTransitions;

    [Header("조건 선택 분기 시 사용")]
    // 조건 선택 분기 (임시로 제작)
    [Tooltip("조건 선택 분기 (미완성)")]
    public ConditionalTransitionList conditionalTransitions; 
}

/// <summary>
/// 노드 전환 방식
/// </summary>
public enum TransitionType
{
    None,
    Sequence,      // 단순히 다음 노드로 넘김(1:1)
    Probability,   // 확률 선택 분기.
    Condition,  // 체력 등의 값 분기 등이 필요. -> 고려해보자
}

/// <summary>
/// 확률 선택 분기 정보
/// </summary>
[System.Serializable]
public class ProbabilityTransition
{
    public EnemyAttackNodeSO nextNode;  // 목표 노드
    [Range(0f, 1f)]
    public float probability;           // 0~1 사이의 확률
}

public enum ConditionOperator
{
    None,            // 조건 없음 (항상 true 반환)
    HpLessThan,      // HP < threshold
    HpGreaterThan    // HP > threshold
}

[System.Serializable]
public class ConditionalTransitionList
{
    public List<ConditionalTransition> conditions;

    [Header("조건 충족 시 목표 노드")]
    public EnemyAttackNodeSO defaultNextNode;
}

[System.Serializable]
public class ConditionalTransition
{
    [Header("현재 조건 타입")]
    public ConditionOperator conditionType;

    [Header("HP 기준 값 입력")]
    public float thresholdValue;

    [Header("조건 충족 시 목표 노드")]
    public EnemyAttackNodeSO conditionedNextNode;

}
