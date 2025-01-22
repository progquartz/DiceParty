using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
    BattleEnd
}
public class BattleManager : SingletonBehaviour<BattleManager>
{

    public BattleState battleState = BattleState.PlayerTurn;

    // 전투에 참여 중인 타겟들 목록 (적, 아군 모두)
    private List<BaseTarget> activeTargets = new List<BaseTarget>();


    public void Init()
    {

    }

    public void OnPlayerTurnEnd()
    {
        if(battleState != BattleState.PlayerTurn)
        {
            Logger.LogWarning($"[BattleManager] - 플레이어 턴이 아님에도 플레이어 턴을 종료하는 조건이 실행되었습니다.");
            return;
        }
        battleState = BattleState.EnemyTurn;
        StartCoroutine(ExecuteEnemyTurn());
    }

    /// <summary>
    /// 적 턴 실행에 필요한 요소들 코루틴 구성
    /// </summary>
    private IEnumerator ExecuteEnemyTurn()
    {
        // 적 턴 실행.

        yield return null;
        // 적 턴이 끝났으면 플레이어 턴으로 복귀
        // 전투 종료 조건 체크(모든 적 사망, 모든 플레이어 사망 등)
        if (CheckBattleEnd())
        {
            battleState = BattleState.BattleEnd;
            yield break;
        }

        battleState = BattleState.PlayerTurn;
        // 이후 플레이어가 행동할 수 있도록 UI 활성화 등
        yield break;
    }

    public void RegisterTarget(BaseTarget target)
    {
        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
            target.OnDead += OnTargetDead; // 사망 이벤트 구독
        }
    }

    private void OnTargetDead(BaseTarget deadTarget)
    {
        // 실제 전투에서 죽은 대상을 제거하거나, UI 갱신 등
        Debug.Log($"BattleManager: {deadTarget.name} 사망 처리");

        if (activeTargets.Contains(deadTarget))
        {
            activeTargets.Remove(deadTarget);
        }

        // 남은 적/아군 체크 후 전투 승리/패배 로직 등
        if (CheckAllEnemiesDead())
        {
            Debug.Log("플레이어 승리!");
        }
    }   

    private bool CheckBattleEnd()
    {
        // 적이나 캐릭터 모두 사망의 경우
        if(CheckAllEnemiesDead() || CheckAllPlayerDead())
        {
            return true;
        }
        return false;
    }

    private bool CheckAllEnemiesDead()
    {
        foreach (var t in activeTargets)
        {
            // BaseEnemy를 상속받은 적이 살아 있으면 false
            if (t is BaseEnemy && t.stat.Hp > 0)
                return false;
        }
        return true;
    }

    private bool CheckAllPlayerDead()
    {
        foreach(var t in activeTargets)
        {
            if(t is BaseCharacter && t.stat.Hp > 0)
                return false;
        }
        return true;
    }

}
