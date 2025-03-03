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

    // ������ ���� ���� Ÿ�ٵ� ��� (��, �Ʊ� ���)
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
        // awake call�� �� ���� ��츦 ���.
        if(DiceRoller == null)
        {
            DiceRoller = FindAnyObjectByType<DiceRoller>();
        }
        DiceRoller.RemoveAllDice();
        DiceRoller.RollAllDiceDummy();
    }

    public void StartBattlePhase(BattleType battleType)
    {
        // ���� �Ϸ� ���°� �ƴѵ�, ȣ��� ���
        if (battleState != BattleState.BattleEnd)
        {
            Logger.LogWarning($"[BattleManager] - ������ �Ϸ���� ���� �����ε� �� ������ ȣ��Ǿ����ϴ�.");
            return;
        }
        currentBattleType = battleType;
        // ���Կ� ��Ͼȵ� �ֵ�(�÷��̾ �����͵�) ���� �����
        OnBattleStart?.Invoke();

        // �� �ε��ϱ�
        EnemyMobListSetting(battleType);

        // �׸��� �� ������ ���� �̷����� �͵� �߰� ����.
        PlayerTurnStart();
    }

    public void PlayerTurnStart()
    {
        battleState = BattleState.PlayerTurn;

        // �÷��̾� �� ����. (��ų �� Ǯ��.)
        OnPlayerTurnStart?.Invoke();

        // �ֻ��� ��� �����ϰ� �����...
        DiceRoller.RollAllDiceNew();
    }

    public void PlayerTurnEnd()
    {
        if (battleState != BattleState.PlayerTurn)
        {
            Logger.LogWarning($"[BattleManager] - �÷��̾� ���� �ƴԿ��� �÷��̾� ���� �����ϴ� ������ ����Ǿ����ϴ�.");
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
            Logger.LogWarning($"[BattleManager] - ���� ���� ����� ���¿��� ���� ���� ");
        }

        battleState = BattleState.BattleEnd;
        
        ResetDiceToDummy();
        OnBattleEnd?.Invoke();

        // �÷��̾ �¸��� ���...
        if(isPlayerWin)
        {
            Debug.LogWarning("�� ����Ʈ ���� ����!");
            // ��� �� ����Ʈ ����. 
            for (int i = activeTargets.Count - 1; i >= 0; i--)
            {
                BaseTarget target = activeTargets[i];
                for (int j = enemyList.Count - 1; j >= 0; j--)
                {
                    BaseTarget activeEnemy = enemyList[j];
                    if (target == activeEnemy)
                    {
                        Debug.LogWarning($"�� {target.name}�� ����Ʈ���� �����մϴ�!");
                        activeTargets.RemoveAt(i);
                        enemyList.RemoveAt(j);
                        Destroy(target.gameObject);
                        break; 
                    }
                }
            }
            // ���� ���� ������ ������ ���...
            if((int)currentBattleType % 10 == 2)
            {
                currentBattleType = BattleType.None;
                MapManager.Instance.GoToNextStage();
                // �������� �Ѿ��.
            }
            
        }
        // ���� �¸��� ���
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
            // ���ӿ��� UI ����.
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
            Logger.LogWarning("[BattleManager] - ���� ȣ�带 ã�� ���߽��ϴ�!");
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

            // ���⿡�� �ִϸ��̼� ����. 

            // yield return new WaitForSeconds(animationTIme);
            // ���⿡ null�� ���Ŀ� �� �ൿ �ִϸ��̼� ���� �� ����ϴ� �ð����� ����.

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

        // �� ���� �������� �÷��̾� ������ ����
        // ���� ���� ���� üũ(��� �� ���, ��� �÷��̾� ��� ��)


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
        Logger.Log($"[BattleManager] {deadTarget.name} ��� ó��");

        // ���� ��/�Ʊ� üũ �� ���� �¸�/�й� ���� ��
        if (CheckAllEnemiesDead())
        {
            Logger.Log("�÷��̾� �¸�!");
            BattleManager.Instance.EndBattlePhase(true);
        }
        
        if(CheckAllPlayerDead())
        {
            Logger.Log("�� �¸�!");
            BattleManager.Instance.EndBattlePhase(false);
        }

    }

    private void OnTargetRemoval(BaseTarget deadTarget)
    {
        // ���� �������� ���� ����� �����ϰų�, UI ���� ��
        Debug.Log($"[BattleManager] {deadTarget.name} ���� ó��");


        if (activeTargets.Contains(deadTarget))
        {
            activeTargets.Remove(deadTarget);
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

    /// <summary>
    /// �������� ��ȣ�� ���� ������ �޾ƿ� ��ƲŸ������ ��ȯ.
    /// </summary>
    /// <param name="stageNumber">�������� ��ȣ</param>
    /// <param name="battleType">������ ���� [0 : �Ϲ� ���� / 1 : ����Ʈ ���� / 2 : ���� ����]</param>
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
            // ���� ���ǵ��� ���� ��� ó��
            Logger.LogError($"���� �������� ����{battleNum}�� BattleType���� ��ȯ�ϴ� �������� ã�� ���߽��ϴ�.");
            return BattleType.Stage1Boss;
            
        }
    }

}
