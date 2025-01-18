using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyAttackNodeSO", menuName = "Scriptable Objects/EnemyAttackNodeSO")]
public class EnemyAttackNodeSO : ScriptableObject
{
    [Header("�� ��忡�� ������ ���� ����Ʈ(1�� �̻� ����)")]
    public List<EnemyAttackDataSO> attacks;

    [Header("���� ���� ���� ����� ����")]
    public TransitionType transitionType;

    // ���� �̵�
    [Tooltip("transitionType�� Sequence�� ��, ���� ��带 ����")]
    public EnemyAttackNodeSO nextNode;

    // Ȯ�� ��� �б�
    [Tooltip("transitionType�� Probability�� ��, ������ ���� �ĺ�")]
    public List<ProbabilityTransition> probabilityTransitions;

    // ���� ��� �б� (�ӽ÷� ����)
    public ConditionalTransitionList conditionalTransitions; 
}

/// <summary>
/// ��� ��ȯ ���
/// </summary>
public enum TransitionType
{
    None,
    Sequence,      // �ܼ��� ���� ���� �Ѿ(1:1)
    Probability,   // Ȯ�� ��� �б�.
    Condition,  // ü�� ���� �� �б� ���� �ʿ�. -> ����غ���
}

/// <summary>
/// Ȯ�� ��� �б� ����
/// </summary>
[System.Serializable]
public class ProbabilityTransition
{
    public EnemyAttackNodeSO nextNode;  // �̵��� ���
    [Range(0f, 1f)]
    public float probability;           // 0~1 ���� Ȯ��
}

public enum ConditionOperator
{
    None,            // ���� ���� (������ true�� ����)
    HpLessThan,      // ��� HP < threshold
    HpGreaterThan    // ��� HP > threshold
}

[System.Serializable]
public class ConditionalTransitionList
{
    public List<ConditionalTransition> conditions;

    [Header("������ �������� ������ �̵��� ���")]
    public EnemyAttackNodeSO defaultNextNode;
}

[System.Serializable]
public class ConditionalTransition
{
    [Header("� ������ üũ����")]
    public ConditionOperator conditionType;

    [Header("HP �񱳿� ����� ���ذ�")]
    public float thresholdValue;

    [Header("������ �����ϸ� �̵��� ���")]
    public EnemyAttackNodeSO conditionedNextNode;

}
