using UnityEngine;

public class BattleEndState : BattleState
{
    private bool isPlayerWin;

    public BattleEndState(BattleManager battleManager, bool isPlayerWin) : base(battleManager)
    {
        this.isPlayerWin = isPlayerWin;
    }

    public override void Enter()
    {
        Debug.Log($"전투 종료 - {(isPlayerWin ? "플레이어 승리" : "플레이어 패배")}");
        battleManager.battleState = BattleStateType.BattleEnd;

        // 주사위 초기화
        battleManager.ResetDiceToDummy();

        if (isPlayerWin)
        {
            HandlePlayerVictory();
        }
        else
        {
            HandlePlayerDefeat();
        }

        battleManager.currentBattleType = BattleType.None;
    }

    public override void Exit()
    {
        // 전투 종료 상태에서는 특별한 종료 처리가 필요 없음
    }

    private void HandlePlayerVictory()
    {
        Debug.LogWarning("적 배틀필드 제거 시작!");
        // 적 배틀필드 제거
        battleManager.ClearEnemyBattlefield();

        // 보상 지급
        if ((int)battleManager.currentBattleType % 10 == 2)
        {
            LootingManager.Instance.OpenLootingTable(battleManager.currentBattleType, true);
        }
        else
        {
            LootingManager.Instance.OpenLootingTable(battleManager.currentBattleType, false);
        }
    }

    private void HandlePlayerDefeat()
    {
        // 플레이어 캐릭터 제거
        battleManager.ClearPlayerBattlefield();

        // 게임오버 UI 표시 등의 처리
        // TODO: 게임오버 처리 추가
    }
}
