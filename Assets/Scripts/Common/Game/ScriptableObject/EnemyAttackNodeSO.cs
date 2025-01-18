using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyAttackNodeSO", menuName = "Scriptable Objects/EnemyAttackNodeSO")]
public class EnemyAttackNodeSO : ScriptableObject
{
    [Header("이 노드에서 실행할 공격 리스트(1개 이상 가능)")]
    public List<EnemyAttackDataSO> attacks;

    [Header("다음 노드로 가는 방법을 정의")]
    public TransitionType transitionType;

    // 순차 이동
    [Tooltip("transitionType이 Sequence일 때, 다음 노드를 연결")]
    public EnemyAttackNodeSO nextNode;

    // 확률 기반 분기
    [Tooltip("transitionType이 Probability일 때, 가능한 여러 후보")]
    public List<ProbabilityTransition> probabilityTransitions;

    // 조건 기반 분기 (임시로 만듬)
    public ConditionalTransitionList conditionalTransitions; 
}

/// <summary>
/// 노드 전환 방식
/// </summary>
public enum TransitionType
{
    None,
    Sequence,      // 단순히 다음 노드로 넘어감(1:1)
    Probability,   // 확률 기반 분기.
    Condition,  // 체력 조건 등 분기 조건 필요. -> 고민해보기
}

/// <summary>
/// 확률 기반 분기 정보
/// </summary>
[System.Serializable]
public class ProbabilityTransition
{
    public EnemyAttackNodeSO nextNode;  // 이동할 노드
    [Range(0f, 1f)]
    public float probability;           // 0~1 사이 확률
}

public enum ConditionOperator
{
    None,            // 조건 없음 (무조건 true로 간주)
    HpLessThan,      // 대상 HP < threshold
    HpGreaterThan    // 대상 HP > threshold
}

[System.Serializable]
public class ConditionalTransitionList
{
    public List<ConditionalTransition> conditions;

    [Header("조건을 만족하지 않으면 이동할 노드")]
    public EnemyAttackNodeSO defaultNextNode;
}

[System.Serializable]
public class ConditionalTransition
{
    [Header("어떤 조건을 체크할지")]
    public ConditionOperator conditionType;

    [Header("HP 비교에 사용할 기준값")]
    public float thresholdValue;

    [Header("조건을 만족하면 이동할 노드")]
    public EnemyAttackNodeSO conditionedNextNode;

}
