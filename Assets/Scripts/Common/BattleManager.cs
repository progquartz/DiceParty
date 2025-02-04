using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
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

    // ������ ���� ���� Ÿ�ٵ� ��� (��, �Ʊ� ���)
    [SerializeField] private List<BaseTarget> activeTargets = new List<BaseTarget>();
    [SerializeField] private List<BaseEnemy> enemyList = new List<BaseEnemy>();
    [SerializeField] private List<BaseCharacter> characterList = new List<BaseCharacter>();

    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private DiceRoller diceRoller;
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
        diceRoller = FindAnyObjectByType<DiceRoller>();
        AddPlayerParty();
    }

    public void TempBattleStart()
    {
        StartBattlePhase(StageType.Stage1Normal);
    }

    public void StartBattlePhase(StageType stageType)
    {
        // ���� �Ϸ� ���°� �ƴѵ�, ȣ��� ���
        if (battleState != BattleState.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - ������ �Ϸ���� ���� �����ε� �� ������ ȣ��Ǿ����ϴ�.");
            return;
        }
        // ���Կ� ��Ͼȵ� �ֵ�(�÷��̾ �����͵�) ���� �����
        OnBattleStart?.Invoke();

        // �� �ε��ϱ�
        EnemyMobListSetting(stageType);

        // �׸��� �� ������ ���� �̷����� �͵� �߰� ����.
        PlayerTurnStart();
    }

    public void PlayerTurnStart()
    {
        battleState = BattleState.PlayerTurn;

        // �÷��̾� �� ����. (��ų �� Ǯ��.)
        OnPlayerTurnStart?.Invoke();

        // �ֻ��� ��� �����ϰ� �����...
        diceRoller.RollAllDiceNew();
    }

    public void PlayerTurnEnd()
    {
        if (battleState != BattleState.PlayerTurn)
        {
            Logger.LogWarning($"[BattleManager] - �÷��̾� ���� �ƴԿ��� �÷��̾� ���� �����ϴ� ������ ����Ǿ����ϴ�.");
            return;
        }

        battleState = BattleState.EnemyTurn;
        // �� �� ���� (��ų ��, ���̽� �����)
        OnPlayerTurnEnd?.Invoke();

        StartCoroutine(ExecuteEnemyTurn());
    }

    

    public void EndBattlePhase()
    {
        if(battleState == BattleState.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - ���� ���� ����� ���¿��� ���� ���� ");
        }
        battleState = BattleState.BattleEnd;
        OnBattleEnd?.Invoke();

    }

    private void EnemyMobListSetting(StageType stageType)
    {
        StageType currentStageType = stageType;

        EnemyHordeDataSO randomHorde = HordeDataLoader.Instance.GetTotalRandomHorde(currentStageType);

        if (randomHorde == null)
        {
            Debug.LogWarning("[BattleManager] - ���� ȣ�带 ã�� ���߽��ϴ�!");
            return;
        }

        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemyList(randomHorde.enemyList, enemyParentTransform);
        }
    }



    /// <summary>
    /// �� �� ���࿡ �ʿ��� ��ҵ� �ڷ�ƾ ����
    /// </summary>
    private IEnumerator ExecuteEnemyTurn()
    {
        // �� �� ����.
        Logger.Log("�� ����!");

        // �� ����... ���� ����. �� �����ӿ� ���� �ʴ� ���.
        foreach(BaseEnemy enemy in enemyList)
        {
            enemy.AttackOnPattern();
            // ���⿡ null�� ���Ŀ� �� �ൿ �ִϸ��̼� ���� �� ����ϴ� �ð����� ����.
            yield return null;
        }

        // �� ���� �������� �÷��̾� ������ ����
        // ���� ���� ���� üũ(��� �� ���, ��� �÷��̾� ��� ��)
        if (CheckBattleEnd())
        {
            EndBattlePhase();
            yield break;
        }

        // �� �� ����.
        Logger.Log("�� ���� ��.");
        PlayerTurnStart();
        // ���� �÷��̾ �ൿ�� �� �ֵ��� UI Ȱ��ȭ ��
        yield break;
    }


    

    [Obsolete("��Ƽ�� �������ִ� ��ũ��Ʈ�� �ۼ��ؼ� �̸� ���������� ��������߸� ��.")]
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
            Logger.LogWarning("[BattleManager] ���������� null���� ���� Ÿ���� ��ϵǷ� �մϴ�.");
            return;
        }

        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
            target.OnDead += OnTargetDead; // ��� �̺�Ʈ ����
            target.OnRemoval += OnTargetRemoval;
        }
    }

    private void OnTargetDead(BaseTarget deadTarget)
    {
        // ���� �������� ���� ����� �����ϰų�, UI ���� ��
        Debug.Log($"[BattleManager] {deadTarget.name} ��� ó��");



        if (activeTargets.Contains(deadTarget))
        {
            activeTargets.Remove(deadTarget);
            if (enemyList.Contains(deadTarget))
            {
                enemyList.Remove(deadTarget as BaseEnemy);
            }
            if (characterList.Contains(deadTarget))
            {
                characterList.Remove(deadTarget as BaseCharacter);
            }
        }

        // ���� ��/�Ʊ� üũ �� ���� �¸�/�й� ���� ��
        if (CheckAllEnemiesDead())
        {
            Debug.Log("�÷��̾� �¸�!");
        }
        
        if(CheckAllPlayerDead())
        {
            Debug.Log("�� �¸�!");
        }

    }

    private void OnTargetRemoval(BaseTarget deadTarget)
    {
        // ���� �������� ���� ����� �����ϰų�, UI ���� ��
        Debug.Log($"[BattleManager] {deadTarget.name} ���� ó��");


        if (activeTargets.Contains(deadTarget))
        {
            activeTargets.Remove(deadTarget);
            if (enemyList.Contains(deadTarget))
            {
                enemyList.Remove(deadTarget as BaseEnemy);
            }
            if(characterList.Contains(deadTarget))
            {
                characterList.Remove(deadTarget as BaseCharacter);
            }
        }

        

        // ���� ��/�Ʊ� üũ �� ���� �¸�/�й� ���� ��
        if (CheckAllEnemiesDead())
        {
            Debug.Log("�÷��̾� �¸�!");
        }


        if (CheckAllPlayerDead())
        {
            Debug.Log("�� �¸�!");
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

    private bool CheckBattleEnd()
    {
        // ���̳� ĳ���� ��� ����� ���
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
            // BaseEnemy�� ��ӹ��� ���� ��� ������ false
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
