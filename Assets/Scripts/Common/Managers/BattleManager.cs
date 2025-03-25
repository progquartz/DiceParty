using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum BattleStateType
{
    PlayerTurn,
    EnemyTurn,
    BattleEnd
}
public class BattleManager : SingletonBehaviour<BattleManager>
{
    [Header("전투의 상태")]
    public BattleStateType battleState = BattleStateType.BattleEnd;
    [Header("전투의 유형")]
    public BattleType currentBattleType = BattleType.None;
    [Header("마지막 전투 유형")]
    public BattleType prevBattleType = BattleType.None;

    private BattleState currentState;

    [Header("전투 참여 타겟 목록")] // (적, 아군 모두)
    [SerializeField] private List<BaseTarget> activeTargets = new List<BaseTarget>(); 
    [SerializeField] private List<BaseEnemy> enemyList = new List<BaseEnemy>();
    [SerializeField] private List<BaseCharacter> characterList = new List<BaseCharacter>();

    [Header("enemySpawner 객체")]
    [SerializeField] private EnemySpawner enemySpawner;
    public DiceRoller DiceRoller {  get; private set; }
    public SkillExecutor SkillExecutor { get; private set; }

    // 추후 리펙토링 때에 다른 코드에 배정.
    [Header("전투 관련 UI")]
    [SerializeField] private Button turnEndButton;

    [Obsolete][SerializeField] private Transform partyParentTransform;
    [Obsolete][SerializeField] private Transform enemyParentTransform;

    public event Action OnBattleStart;
    public event Action OnPlayerTurnStart;
    public event Action OnPlayerTurnEnd;
    public event Action OnEnemyTurnStart;
    public event Action OnEnemyTurnEnd;
    public event Action OnBattleEnd;

    protected override void Init()
    {
        base.Init();
        DiceRoller = FindAnyObjectByType<DiceRoller>();
        SkillExecutor = new SkillExecutor();
        InitUI();
        AddPlayerParty();
    }

    public void ChangeState(BattleState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();

        // 상태가 BattleEndState로 변경될 때 OnBattleEnd 이벤트 발생
        if (currentState is BattleEndState)
        {
            OnBattleEnd?.Invoke();
        }
    }



    public void StartBattlePhase(BattleType battleType)
    {
        // 전투 완료 상태가 아닌데, 호출된 경우
        if (battleState != BattleStateType.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - 전투가 완료되지 않은 상태인데 새 전투가 호출되었습니다.");
            return;
        }

        currentBattleType = battleType;
        OnBattleStart?.Invoke();

        DiceRoller.RemoveAllDice();
        EnemyMobListSetting(battleType);

        turnEndButton.gameObject.SetActive(true);

        Logger.Log($"전투 시작");
        ChangeState(new PlayerTurnState(this));
    }

    public void StartPlayerTurn()
    {
        battleState = BattleStateType.PlayerTurn;
        OnPlayerTurnStart?.Invoke();
    }


    public void EndPlayerTurn()
    {
        OnPlayerTurnEnd?.Invoke();
    }

    public void StartEnemyTurn()
    {
        battleState = BattleStateType.EnemyTurn;
        OnEnemyTurnStart?.Invoke();
    }

    public void EndEnemyTurn()
    {
        OnEnemyTurnEnd?.Invoke();
    }

    public void ClearEnemyBattlefield()
    {
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
    }

    public void ClearPlayerBattlefield()
    {
        for (int i = activeTargets.Count - 1; i >= 0; i--)
        {
            BaseTarget target = activeTargets[i];
            for (int j = characterList.Count - 1; j >= 0; j--)
            {
                BaseTarget activeCharacter = characterList[j];
                if (target == activeCharacter)
                {
                    Debug.LogWarning($"캐릭터 {target.name}을 배틀필드에서 제거합니다!");
                    activeTargets.RemoveAt(i);
                    characterList.RemoveAt(j);
                    Destroy(target.gameObject);
                    break;
                }
            }
        }
    }



    public void EndBattlePhase(bool isPlayerWin)
    {
        if (battleState == BattleStateType.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - 이미 전투 종료된 상태에서 전투 종료 ");
            return;
        }

        turnEndButton.gameObject.SetActive(false);

        ChangeState(new BattleEndState(this, isPlayerWin));
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

        if (enemySpawner != null)
        {
            Logger.Log($"{randomHorde.name} Horde로 소환합니다."); 
            enemySpawner.SpawnEnemyList(randomHorde.enemyList, enemyParentTransform);
        }
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

    private void InitUI()
    {
        turnEndButton.onClick.AddListener(OnPushTurnEndButton);
        turnEndButton.gameObject.SetActive(false);
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

        currentState?.OnTargetDied(deadTarget);
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

    public void OnPushTurnEndButton()
    {
        if (battleState == BattleStateType.PlayerTurn)
        {
            ChangeState(new EnemyTurnState(this));
        }
        else
        {
            Logger.Log("플레이어의 턴이 아니기 때문에 턴을 넘길 수 없습니다.");
        }
    }

    public List<BaseCharacter> GetCharacterList()
    {
        return characterList;
    }

    public List<BaseEnemy> GetEnemyList()
    {
        return enemyList;
    }

    public bool CheckAllEnemiesDead()
    {
        foreach (var t in activeTargets)
        {
            // BaseEnemy 체크 후 승리 처리 하기
            if (t is BaseEnemy && t.stat.Hp > 0)
                return false;
        }
        return true;
    }

    public bool CheckAllPlayerDead()
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
    public static BattleType ConvertToBattleType(int stageNumber, int battleType)
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

    public static int ConvertTobattleTypeInt(BattleType battleType)
    {
        return (int)battleType % 10;
    }

}
