using UnityEngine;

public class PlayerTurnState : BattleState
{
    public PlayerTurnState(BattleManager battleManager) : base(battleManager) { }

    public override void Enter()
    {
        Debug.Log("플레이어 턴 시작");
        battleManager.StartPlayerTurn();

        // 주사위 초기화 및 굴리기
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
        // 스킬 실행 후 필요한 처리
        // 예: 스킬 사용 후 상태 체크, 추가 효과 등
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
        // 주사위가 굴려진 후의 처리
        // UI 업데이트는 이벤트를 통해 처리됨
    }
}
