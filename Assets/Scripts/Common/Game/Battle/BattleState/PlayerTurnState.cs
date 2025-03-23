using UnityEngine;

public class PlayerTurnState : BattleState
{
    public PlayerTurnState(BattleManager battleManager) : base(battleManager) { }

    public override void Enter()
    {
        Debug.Log("�÷��̾� �� ����");
        battleManager.StartPlayerTurn();

        // �ֻ��� �ʱ�ȭ �� ������
        battleManager.DiceRoller.RollAllDiceNew();
    }

    public override void Exit()
    {
        battleManager.EndPlayerTurn();
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
        // ��ų ���� �� �ʿ��� ó��
        // ��: ��ų ��� �� ���� üũ, �߰� ȿ�� ��
        if (battleManager.CheckAllEnemiesDead())
        {
            battleManager.EndBattlePhase(true);
        }
        else if (battleManager.CheckAllPlayerDead())
        {
            battleManager.EndBattlePhase(false);
        }
        else if (executor is BaseCharacter)
        {

        }
    }

    public override void OnDiceRolled()
    {
        // �ֻ����� ������ ���� ó��
        // UI ������Ʈ�� �̺�Ʈ�� ���� ó����
    }
}
