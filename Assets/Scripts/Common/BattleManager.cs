using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
    BattleEnd
}
public class BattleManager : SingletonBehaviour<BattleManager>
{
    public BattleState battleState = BattleState.BattleEnd;

    // 전투에 참여 중인 타겟들 목록 (적, 아군 모두)
    [SerializeField] private List<BaseTarget> activeTargets = new List<BaseTarget>();

    [SerializeField] private EnemySpawner enemySpawner;
    [Obsolete] [SerializeField] private Transform partyParentTransform;
    [Obsolete][SerializeField] private Transform enemyParentTransform;

    private void Awake()
    {
        base.Init();
        Init();
    }

    public void Init()
    {
        AddPlayerParty();
    }

    public void TempBattleStart()
    {
        StartBattlePhase(StageType.Stage1Normal);
    }

    public void StartBattlePhase(StageType stageType)
    {
        // 플레이어 턴 가정.
        battleState = BattleState.PlayerTurn;

        // 이후에 스킬들 락 걸고 슬롯에 등록안된 애들(플레이어가 버린것들) 전부 지우고
        EraseAllNonUsingSkill();

        // 주사위 사용 가능하게 만들고...
        

        // 적 로드하기
        EnemyMobListSetting(stageType);

    }

    private void EraseAllNonUsingSkill()
    {
        // 리소스 감당 되는지 추후에 체크 필요.
        List<SkillUI> allSkill = UnityEngine.Object.FindObjectsByType<SkillUI>(FindObjectsSortMode.None).ToList();

        foreach (SkillUI skill in allSkill)
        {
            if(!skill.IsAttachedToSkillUISlot())
            {
                skill.DestorySelf();
            }
        }
    }

    private void EnemyMobListSetting(StageType stageType)
    {
        StageType currentStageType = stageType;

        EnemyHordeDataSO randomHorde = HordeDataLoader.Instance.GetTotalRandomHorde(currentStageType);

        if (randomHorde == null)
        {
            Debug.LogWarning("랜덤 호드를 찾지 못했습니다!");
            return;
        }

        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemyList(randomHorde.enemyList, enemyParentTransform);
        }
    }



    public void OnPlayerTurnEnd()
    {
        if (battleState != BattleState.PlayerTurn)
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
        Logger.Log("적 공격!");

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



    [Obsolete("파티를 관리해주는 스크립트를 작성해서 이를 개별적으로 관리해줘야만 함.")]
    private void AddPlayerParty()
    {
        BaseCharacter[] targets = partyParentTransform.GetComponentsInChildren<BaseCharacter>();
        foreach (BaseCharacter target in targets)
        {
            RegisterTarget(target);
        }
    }

    private void AddEnemyList(List<BaseEnemy> enemyList)
    {
        foreach (BaseEnemy target in enemyList)
        {
            RegisterTarget(target);
        }
    }

    private void AddEnemy(BaseEnemy enemy)
    {
        RegisterTarget(enemy);
    }


    public void RegisterTarget(BaseTarget target)
    {
        if (target == null)
        {
            Logger.LogWarning("[BattleManager] 비정상적인 null값을 가진 타겟이 등록되려 합니다.");
            return;
        }

        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
            target.OnDead += OnTargetDead; // 사망 이벤트 구독
            target.OnRemoval += OnTargetRemoval;
        }
    }

    private void OnTargetDead(BaseTarget deadTarget)
    {
        // 실제 전투에서 죽은 대상을 제거하거나, UI 갱신 등
        Debug.Log($"[BattleManager] {deadTarget.name} 사망 처리");

        // 남은 적/아군 체크 후 전투 승리/패배 로직 등
        if (CheckAllEnemiesDead())
        {
            Debug.Log("플레이어 승리!");
        }
        
        if(CheckAllPlayerDead())
        {
            Debug.Log("적 승리!");
        }

    }

    private void OnTargetRemoval(BaseTarget deadTarget)
    {
        // 실제 전투에서 죽은 대상을 제거하거나, UI 갱신 등
        Debug.Log($"[BattleManager] {deadTarget.name} 삭제 처리");


        if (activeTargets.Contains(deadTarget))
        {
            activeTargets.Remove(deadTarget);
        }

        // 남은 적/아군 체크 후 전투 승리/패배 로직 등
        if (CheckAllEnemiesDead())
        {
            Debug.Log("플레이어 승리!");
        }


        if (CheckAllPlayerDead())
        {
            Debug.Log("적 승리!");
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
