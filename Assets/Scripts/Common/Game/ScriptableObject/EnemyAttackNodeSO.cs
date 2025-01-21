using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyAttackNodeSO", menuName = "Scriptable Objects/EnemyAttackNodeSO")]
public class EnemyAttackNodeSO : ScriptableObject
{
    [Header("�� ��忡�� ������ ���� ����Ʈ(1�� �̻� ����)")]
    [Header("���ݸ�")]
    public string attackName;

    // �������� �𸣰ڴٸ���...
    [Header("���� �� �ϰ� �Ǵ� ���")]
    [TextArea] public string attackText;

    [Header("��ų�� ������ �Ǵ� ����Ʈ")]
    public List<SkillEffectData> attackEffects;


    // ���� ��� ����

    [Header("���� ���� ���� ����� ����")]
    public TransitionType transitionType;

    // ���� �̵�
    [Tooltip("���� ������ ��, ���� ��带 ����")]
    public EnemyAttackNodeSO nextNode;


    [Header("Ȯ�� ��� �б� �� ���")]
    // Ȯ�� ��� �б�
    [Tooltip("������ Ȯ�� ����϶� Ȯ��")]
    public List<ProbabilityTransition> probabilityTransitions;

    [Header("���� ��� �б� �� ���")]
    // ���� ��� �б� (�ӽ÷� ����)
    [Tooltip("���� ��� �б� (�̿ϼ�)")]
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
