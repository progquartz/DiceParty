using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyPatternExecutor
{
    private EnemyAttackPatternSO _currentPattern;
    private EnemyAttackNodeSO _currentNode;

    private SkillExecutor _skillExecutor;

    // 패턴 실행 시작
    public void StartPattern(EnemyAttackPatternSO pattern)
    {
        _currentPattern = pattern;
        _currentNode = pattern.rootNode;
    }

    // BattleManager에 있어서 매턴 호출 필요.
    public void ExecuteNextAttack()
    {
        if (_currentNode == null)
        {
            _currentNode = _currentPattern.rootNode;
            Logger.Log($"{_currentPattern.patternName}에서 더 이상 진행할 노드가 없기에, 패턴을 루트로 되돌립니다.");
        }

        ExecuteAttack(_currentNode);

        // 다음 노드 체크.
        _currentNode = GetNextNode(_currentNode);
    }

    private void ExecuteAttack(EnemyAttackNodeSO attackData)
    {
        Debug.Log($"적이 공격 {attackData.attackName}을(를) 사용합니다.");

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

    // 확률 분기 처리
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
        // 만약 확률 합이 1.0 미만이면, 마지막 노드를 default로 반환
        return transitions.Count > 0 ? transitions[transitions.Count - 1].nextNode : null;
    }

    [Obsolete("내부 구현 안됨!")]
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
