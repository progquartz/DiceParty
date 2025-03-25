using System.Collections;
using UnityEngine;

public class EnemyTurnState : BattleState
{
    public EnemyTurnState(BattleManager battleManager) : base(battleManager) { }

    public override void Enter()
    {
        Debug.Log("�� �� ����");
        battleManager.StartEnemyTurn();

        battleManager.StartCoroutine(ExecuteEnemyActions());
    }

    public override void Exit()
    {
        battleManager.EndEnemyTurn();
    }

    private IEnumerator ExecuteEnemyActions()
    {
        foreach (BaseEnemy enemy in battleManager.GetEnemyList())
        {
            if (enemy == null || enemy.stat.Hp <= 0) continue;

            enemy.AttackOnPattern();

            // ���⿡ �ִϸ��̼� ��� �ð� �߰� ����
            //yield return new WaitForSeconds(animationTime);

            if (battleManager.CheckAllEnemiesDead())
            {
                battleManager.EndBattlePhase(true);
                yield break;
            }
            if (battleManager.CheckAllPlayerDead())
            {
                battleManager.EndBattlePhase(false);
                yield break;
            }

            yield return null;
        }

        // ��� ���� �ൿ�� ������ �÷��̾� ������ ��ȯ
        battleManager.ChangeState(new PlayerTurnState(battleManager));
    }

    public override void OnTargetDied(BaseTarget target)
    {
        if (battleManager.CheckAllEnemiesDead())
        {
            battleManager.EndBattlePhase(true);
        }
        else if (battleManager.CheckAllPlayerDead())
        {
            battleManager.EndBattlePhase(false);
        }
    }

    public override void OnSkillExecuted(BaseTarget executor, BaseTarget target)
    {
        if (battleManager.CheckAllEnemiesDead())
        {
            battleManager.EndBattlePhase(true);
        }
        else if (battleManager.CheckAllPlayerDead())
        {
            battleManager.EndBattlePhase(false);
        }
    }
}
