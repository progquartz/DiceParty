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
    public SkillExecutor SkillExecutor;

    // 전투에 참여 중인 타겟들 목록 (적, 아군 모두)
    [SerializeField] private List<BaseTarget> activeTargets = new List<BaseTarget>();
    [SerializeField] private List<BaseEnemy> enemyList = new List<BaseEnemy>();
    [SerializeField] private List<BaseCharacter> characterList = new List<BaseCharacter>();

    [SerializeField] private EnemySpawner enemySpawner;

    [Obsolete][SerializeField] private Transform partyParentTransform;
    [Obsolete][SerializeField] private Transform enemyParentTransform;

    public event Action OnBattleStart;
    public event Action OnPlayerTurnStart;
    public event Action OnPlayerTurnEnd;
    public event Action OnEnemyTurnStart;
    public event Action OnEnemyTurnEnd;
    public event Action OnBattleEnd;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        DiceRoller = FindAnyObjectByType<DiceRoller>();
        SkillExecutor = new SkillExecutor();
        AddPlayerParty();
    }

    public void ResetDiceToDummy()
    {
        // awake call이 안 된 경우를 대비
        if (DiceRoller == null)
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
        // 전투에 참여안된 것들(플레이어 캐릭터들) 모두 등록하기
        OnBattleStart?.Invoke();

        DiceRoller.RemoveAllDice();
        // 적 로드하기
        EnemyMobListSetting(battleType);

        // 그리고 새 전투를 위해 이루어질 것들 추가 진행.
        PlayerTurnStart();
    }

    public void PlayerTurnStart()
    {
        battleState = BattleState.PlayerTurn;

        // 플레이어 턴 시작. (스킬 풀 풀기.)
        OnPlayerTurnStart?.Invoke();

        // 주사위 모두 활성화고 굴리기...
        DiceRoller.RollAllDiceNew();
    }

    public void PlayerTurnEnd()
    {
        if (battleState != BattleState.PlayerTurn)
        {
            Logger.LogWarning($"[BattleManager] - 플레이어 턴이 아님에도 플레이어 턴을 종료하는 명령이 실행되었습니다.");
            return;
        }

        battleState = BattleState.EnemyTurn;

        OnPlayerTurnEnd?.Invoke();

        StartCoroutine(ExecuteEnemyTurn());
    }



    public void EndBattlePhase(bool isPlayerWin)
    {
        if (battleState == BattleState.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - 이미 전투 종료된 상태에서 전투 종료 ");
        }

        battleState = BattleState.BattleEnd;

        ResetDiceToDummy();
        OnBattleEnd?.Invoke();

        // 플레이어가 승리한 경우...
        if (isPlayerWin)
        {
            Debug.LogWarning("적 배틀필드 제거 시작!");
            // 모든 적 배틀필드 제거. 
            for (int i = activeTargets.Count - 1; i >= 0; i--)
            {
                BaseTarget target = activeTargets[i];
                for (int j = enemyList.Count - 1; j >= 0; j--)
                {
                    BaseTarget activeEnemy = enemyList[j];
                    if (target == activeEnemy)
                    {
                        Debug.LogWarning($"적 {target.name}을 배틀필드에서 제거합니다!");
                        activeTargets.RemoveAt(i);
                        enemyList.RemoveAt(j);
                        Destroy(target.gameObject);
                        break;
                    }
                }
            }
            // 보스 전투 승리한 경우에는 보상이 다름...
            if ((int)currentBattleType % 10 == 2)
            {
                LootingManager.Instance.OpenLootingTable(currentBattleType, true);
            }
            else
            {
                LootingManager.Instance.OpenLootingTable(currentBattleType, false);
            }

        }
        // 적이 승리한 경우
        else
        {
            for (int i = activeTargets.Count - 1; i >= 0; i--)
            {
                BaseTarget target = activeTargets[i];
                for (int j = characterList.Count - 1; j >= 0; j--)
                {
                    BaseTarget activeEnemy = enemyList[j];
                    if (target == activeEnemy)
                    {
                        Debug.LogWarning($"아군 캐릭터 {target.name}을 배틀필드에서 제거합니다!");
                        activeTargets.RemoveAt(i);
                        characterList.RemoveAt(j);
                        Destroy(target.gameObject);
                        break;
                    }
                }
            }
            // 게임오버 UI 출력.
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
            Logger.LogWarning("[BattleManager] - 적당한 호드를 찾지 못했습니다!");
            return;
        }

        foreach(EnemyDataSO enemyDataSO in randomHorde.enemyList)
        {
            if(enemyDataSO == null)
            {
                Debug.LogError($"{randomHorde.name}의 데이터가 null로 나옴.");
            }
        }

        if (enemySpawner != null)
        {
            Logger.Log($"{randomHorde.name} Horde로 소환합니다."); 
            enemySpawner.SpawnEnemyList(randomHorde.enemyList, enemyParentTransform);
        }
    }



    /// <summary>
    /// 적 턴 실행에 필요한 요소들 코루틴 실행
    /// </summary>
    private IEnumerator ExecuteEnemyTurn()
    {
        OnEnemyTurnStart?.Invoke();
        // 적 턴 시작.
        Logger.Log("적 턴!");

        // 적 공격... 순서 실행. 한 프레임에 모두 하지 않는 것임.
        foreach (BaseEnemy enemy in enemyList)
        {
            enemy.AttackOnPattern();

            // 여기에서 애니메이션 실행. 

            // yield return new WaitForSeconds(animationTIme);
            // 여기에 null이 나중에 각 행동 애니메이션 실행 후 종료하는 시간까지 대기.

            if (CheckAllEnemiesDead())
            {
                EndBattlePhase(true);
                yield break;
            }
            if (CheckAllPlayerDead())
            {
                EndBattlePhase(false);
                yield break;
            }

            yield return null;
        }

        // 적 공격 끝났으면 플레이어 차례로 전환
        // 전투 종료 조건 체크(모든 적 사망, 모든 플레이어 사망 등)


        // 적 턴 종료.
        OnEnemyTurnEnd?.Invoke();
        Logger.Log("적 턴이 끝.");

        PlayerTurnStart();
        // 이제 플레이어가 행동할 수 있도록 UI 활성화 등
        yield break;
    }




    [Obsolete("이미 사용하지 않는 컴포넌트를 작성하여 이를 대체하도록 수정해야 합니다.")]
    private void AddPlayerParty()
    {
        if(partyParentTransform != null)
        {
            BaseCharacter[] targets = partyParentTransform.GetComponentsInChildren<BaseCharacter>();
            foreach (BaseCharacter target in targets)
            {
                characterList.Add(target);
                RegisterTarget(target);
            }
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
            Logger.LogWarning("[BattleManager] 등록하려는 타겟이 null값인 채로 등록됩니다.");
            return;
        }

        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
            target.OnDead += OnTargetDead; // 죽음 이벤트 연결
            target.OnRemoval += OnTargetRemoval;
        }
    }

    private void OnTargetDead(BaseTarget deadTarget)
    {
        // 죽은 타겟에 대한 처리를 수행하거나, UI 갱신 등
        Logger.Log($"[BattleManager] {deadTarget.name} 죽음 처리");

        // 죽음 후/이전 체크 후 승리/패배 처리 하기
        if (CheckAllEnemiesDead())
        {
            Logger.Log("플레이어 승리!");
            BattleManager.Instance.EndBattlePhase(true);
        }

        if (CheckAllPlayerDead())
        {
            Logger.Log("적 승리!");
            BattleManager.Instance.EndBattlePhase(false);
        }

    }

    private void OnTargetRemoval(BaseTarget deadTarget)
    {
        // 죽은 타겟에 대한 처리를 수행하거나, UI 갱신 등
        Debug.Log($"[BattleManager] {deadTarget.name} 제거 처리");


        if (activeTargets.Contains(deadTarget))
        {
            activeTargets.Remove(deadTarget);
        }



        // 죽음 후/이전 체크 후 승리/패배 처리 하기
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
            // BaseEnemy 체크 후 승리 처리 하기
            if (t is BaseEnemy && t.stat.Hp > 0)
                return false;
        }
        return true;
    }

    private bool CheckAllPlayerDead()
    {
        foreach (var t in activeTargets)
        {
            if (t is BaseCharacter && t.stat.Hp > 0)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 스테이지 번호를 입력받아 전투 타입으로 변환.
    /// </summary>
    /// <param name="stageNumber">스테이지 번호</param>
    /// <param name="battleType">전투 타입 [0 : 일반 전투 / 1 : 테스트 전투 / 2 : 보스 전투]</param>
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
            // 잘못된 입력에 대한 처리
            Logger.LogError($"잘못된 전투 번호 {battleNum}를 BattleType으로 변환할 수 없습니다.");
            return BattleType.Stage1Boss;

        }
    }

}
