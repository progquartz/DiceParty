using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Schema;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum BattleState
{
    PlayerTurn,
    EnemyTurn,
    BattleEnd
}
public class BattleManager : SingletonBehaviour<BattleManager>
{
    public BattleState battleState = BattleState.BattleEnd;
    public BattleType currentBattleType = BattleType.None;

    public DiceRoller DiceRoller;

    // 전투에 참여 중인 타겟들 목록 (적, 아군 모두)
    [SerializeField] private List<BaseTarget> activeTargets = new List<BaseTarget>();
    [SerializeField] private List<BaseEnemy> enemyList = new List<BaseEnemy>();
    [SerializeField] private List<BaseCharacter> characterList = new List<BaseCharacter>();

    [SerializeField] private EnemySpawner enemySpawner;
    
    [Obsolete] [SerializeField] private Transform partyParentTransform;
    [Obsolete][SerializeField] private Transform enemyParentTransform;

    public event Action OnBattleStart;
    public event Action OnPlayerTurnStart;
    public event Action OnPlayerTurnEnd;
    public event Action OnBattleEnd;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        DiceRoller = FindAnyObjectByType<DiceRoller>();
        AddPlayerParty();
    }

    public void ResetDiceToDummy()
    {
        // awake call이 더 느릴 경우를 대비.
        if(DiceRoller == null)
        {
            DiceRoller = FindAnyObjectByType<DiceRoller>();
        }
        DiceRoller.RemoveAllDice();
        DiceRoller.RollAllDiceDummy();
    }

    public void StartBattlePhase(BattleType battleType)
    {
        // 전투 완료 상태가 아닌데, 호출된 경우
        if (battleState != BattleState.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - 전투가 완료되지 않은 상태인데 새 전투가 호출되었습니다.");
            return;
        }
        currentBattleType = battleType;
        // 슬롯에 등록안된 애들(플레이어가 버린것들) 전부 지우고
        OnBattleStart?.Invoke();

        // 적 로드하기
        EnemyMobListSetting(battleType);

        // 그리고 턴 시작할 때에 이뤄지는 것들 추가 진행.
        PlayerTurnStart();
    }

    public void PlayerTurnStart()
    {
        battleState = BattleState.PlayerTurn;

        // 플레이어 턴 가정. (스킬 락 풀림.)
        OnPlayerTurnStart?.Invoke();

        // 주사위 사용 가능하게 만들고...
        DiceRoller.RollAllDiceNew();
    }

    public void PlayerTurnEnd()
    {
        if (battleState != BattleState.PlayerTurn)
        {
            Logger.LogWarning($"[BattleManager] - 플레이어 턴이 아님에도 플레이어 턴을 종료하는 조건이 실행되었습니다.");
            return;
        }

        battleState = BattleState.EnemyTurn;
        
        OnPlayerTurnEnd?.Invoke();

        StartCoroutine(ExecuteEnemyTurn());
    }

    

    public void EndBattlePhase(bool isPlayerWin)
    {
        if(battleState == BattleState.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - 전투 턴이 종료된 상태에서 전투 종료 ");
        }

        battleState = BattleState.BattleEnd;
        
        ResetDiceToDummy();
        OnBattleEnd?.Invoke();

        // 플레이어가 승리일 경우...
        if(isPlayerWin)
        {
            Debug.LogWarning("적 리스트 제거 시작!");
            // 모든 적 리스트 제거. 
            for (int i = activeTargets.Count - 1; i >= 0; i--)
            {
                BaseTarget target = activeTargets[i];
                for (int j = enemyList.Count - 1; j >= 0; j--)
                {
                    BaseTarget activeEnemy = enemyList[j];
                    if (target == activeEnemy)
                    {
                        Debug.LogWarning($"적 {target.name}을 리스트에서 제거합니다!");
                        activeTargets.RemoveAt(i);
                        enemyList.RemoveAt(j);
                        Destroy(target.gameObject);
                        break; 
                    }
                }
            }
            // 만약 전투 유형이 보스일 경우...
            if((int)currentBattleType % 10 == 2)
            {
                currentBattleType = BattleType.None;
                MapManager.Instance.GoToNextStage();
                // 스테이지 넘어가기.
            }
            
        }
        // 적이 승리일 경우
        else
        {
            foreach(BaseTarget target in activeTargets)
            {
                foreach(BaseTarget activeCharacter in characterList)
                {
                    if (target == activeCharacter)
                    {
                        activeTargets.Remove(target);
                        characterList.Remove(activeCharacter as BaseCharacter);
                    }
                }
            }
            // 게임오버 UI 송출.
            currentBattleType = BattleType.None;
        }
        currentBattleType = BattleType.None;
    }

    private void EnemyMobListSetting(BattleType stageType)
    {
        BattleType currentStageType = stageType;

        EnemyHordeDataSO randomHorde = HordeDataLoader.Instance.GetTotalRandomHorde(currentStageType);

        if (randomHorde == null)
        {
            Logger.LogWarning("[BattleManager] - 랜덤 호드를 찾지 못했습니다!");
            return;
        }

        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemyList(randomHorde.enemyList, enemyParentTransform);
        }
    }



    /// <summary>
    /// 적 턴 실행에 필요한 요소들 코루틴 구성
    /// </summary>
    private IEnumerator ExecuteEnemyTurn()
    {
        // 적 턴 실행.
        Logger.Log("적 공격!");

        // 적 마다... 순서 진행. 적 프레임에 맞춰 초당 대기.
        foreach(BaseEnemy enemy in enemyList)
        {
            enemy.AttackOnPattern();

            // 여기에서 애니메이션 수행. 

            // yield return new WaitForSeconds(animationTIme);
            // 여기에 null은 이후에 적 행동 애니메이션 실행 후 대기하는 시간으로 수정.

            if(CheckAllEnemiesDead())
            {
                EndBattlePhase(true);
                yield break;
            }
            if(CheckAllPlayerDead())
            {
                EndBattlePhase(false);
                yield break;
            }

            yield return null;
        }

        // 적 턴이 끝났으면 플레이어 턴으로 복귀
        // 전투 종료 조건 체크(모든 적 사망, 모든 플레이어 사망 등)


        // 적 턴 실행.
        Logger.Log("적 공격 끝.");
        PlayerTurnStart();
        // 이후 플레이어가 행동할 수 있도록 UI 활성화 등
        yield break;
    }


    

    [Obsolete("파티를 관리해주는 스크립트를 작성해서 이를 개별적으로 관리해줘야만 함.")]
    private void AddPlayerParty()
    {
        BaseCharacter[] targets = partyParentTransform.GetComponentsInChildren<BaseCharacter>();
        foreach (BaseCharacter target in targets)
        {
            characterList.Add(target);
            RegisterTarget(target);
        }
    }

    public void AddEnemy(BaseEnemy enemy)
    {
        enemyList.Add(enemy);
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
        Logger.Log($"[BattleManager] {deadTarget.name} 사망 처리");

        // 남은 적/아군 체크 후 전투 승리/패배 로직 등
        if (CheckAllEnemiesDead())
        {
            Logger.Log("플레이어 승리!");
            BattleManager.Instance.EndBattlePhase(true);
        }
        
        if(CheckAllPlayerDead())
        {
            Logger.Log("적 승리!");
            BattleManager.Instance.EndBattlePhase(false);
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

    

    public List<BaseCharacter> GetAllCharacters()
    {
        return characterList;
    }

    public List<BaseEnemy> GetAllEnemys()
    {
        return enemyList;
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

    /// <summary>
    /// 스테이지 번호와 전투 유형을 받아와 배틀타입으로 변환.
    /// </summary>
    /// <param name="stageNumber">스테이지 번호</param>
    /// <param name="battleType">전투의 유형 [0 : 일반 전투 / 1 : 엘리트 전투 / 2 : 보스 전투]</param>
    /// <returns></returns>
    public static BattleType ConvertToStageType(int stageNumber, int battleType)
    {
        int battleNum = stageNumber * 10 + battleType;
        if (Enum.IsDefined(typeof(BattleType), battleNum))
        {
            BattleType enumValue = (BattleType)battleNum;
            return enumValue;
        }
        else
        {
            // 값이 정의되지 않은 경우 처리
            Logger.LogError($"현재 스테이지 단위{battleNum}을 BattleType으로 변환하는 과정에서 찾지 못했습니다.");
            return BattleType.Stage1Boss;
            
        }
    }

}
