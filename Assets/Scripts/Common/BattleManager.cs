using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonBehaviour<BattleManager>
{

    // 전투에 참여 중인 타겟들 목록 (적, 아군 모두)
    private List<BaseTarget> activeTargets = new List<BaseTarget>();

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

    private bool CheckAllEnemiesDead()
    {
        foreach (var t in activeTargets)
        {
            // BaseEnemy를 상속받은 적이 살아 있으면 false
            if (t is BaseEnemy && t.Hp > 0)
                return false;
        }
        return true;
    }

}
