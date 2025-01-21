using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyPatternExecutor
{
    private EnemyAttackPatternSO _currentPattern;
    private EnemyAttackNodeSO _currentNode;

    private SkillExecutor _skillExecutor;

    // ���� ���� ����
    public void StartPattern(EnemyAttackPatternSO pattern)
    {
        _currentPattern = pattern;
        _currentNode = pattern.rootNode;
    }

    // BattleManager�� �־ ���� ȣ�� �ʿ�.
    public void ExecuteNextAttack()
    {
        if (_currentNode == null)
        {
            _currentNode = _currentPattern.rootNode;
            Logger.Log($"{_currentPattern.patternName}���� �� �̻� ������ ��尡 ���⿡, ������ ��Ʈ�� �ǵ����ϴ�.");
        }

        ExecuteAttack(_currentNode);

        // ���� ��� üũ.
        _currentNode = GetNextNode(_currentNode);
    }

    private void ExecuteAttack(EnemyAttackNodeSO attackData)
    {
        Debug.Log($"���� ���� {attackData.attackName}��(��) ����մϴ�.");

        SkillExecutor skillExecutor = _skillExecutor;
        foreach(SkillEffectData skillData in attackData.attackEffects) 
        {
            skillExecutor.UseSkill(skillData);
        }

    }

    private EnemyAttackNodeSO GetNextNode(EnemyAttackNodeSO node)
    {
        switch (node.transitionType)
        {
            case TransitionType.Sequence:
                return node.nextNode;

            case TransitionType.Probability:
                return GetNextNodeByProbability(node.probabilityTransitions);
            case TransitionType.Condition:
                return GetNextNodeByCondition(node);

            default:
                return null;
        }
    }

    // Ȯ�� �б� ó��
    private EnemyAttackNodeSO GetNextNodeByProbability(List<ProbabilityTransition> transitions)
    {
        float rand = UnityEngine.Random.value; // 0~1
        float cumulative = 0f;
        foreach (var t in transitions)
        {
            cumulative += t.probability;
            if (rand <= cumulative)
            {
                return t.nextNode;
            }
        }
        // ���� Ȯ�� ���� 1.0 �̸��̸�, ������ ��带 default�� ��ȯ
        return transitions.Count > 0 ? transitions[transitions.Count - 1].nextNode : null;
    }

    [Obsolete("���� ���� �ȵ�!")]
    private EnemyAttackNodeSO GetNextNodeByCondition(EnemyAttackNodeSO node)
    {
        EnemyAttackNodeSO nextAttackNode = node.conditionalTransitions.defaultNextNode;
        foreach(ConditionalTransition transition in node.conditionalTransitions.conditions)
        {
            switch (transition.conditionType)
            {
                case ConditionOperator.HpLessThan:

                    break;
                case ConditionOperator.HpGreaterThan:

                    break;

                case ConditionOperator.None:
                    nextAttackNode = transition.conditionedNextNode;
                    break;

            }
        }

        return nextAttackNode;
        
    }
}
