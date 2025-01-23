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

    public BattleState battleState = BattleState.BattleEnd;

    // ������ ���� ���� Ÿ�ٵ� ��� (��, �Ʊ� ���)
    [SerializeField] private List<BaseTarget> activeTargets = new List<BaseTarget>();


    [Obsolete] [SerializeField] private Transform partyParentTransform;

    private void Awake()
    {
        base.Init();
        Init();
    }

    public void Init()
    {
        AddPlayerParty();
    }

    public void StartBattlePhase()
    {
        battleState = BattleState.PlayerTurn;
        // ���Ŀ� ��ų�� �� �ɰ� ���Կ� ��Ͼȵ� �ֵ�(�÷��̾ �����͵�) ���� �����

        // �ֻ��� ��� �����ϰ� �����...

        // �� �ε��ϱ�


    }

    public void OnPlayerTurnEnd()
    {
        if (battleState != BattleState.PlayerTurn)
        {
            Logger.LogWarning($"[BattleManager] - �÷��̾� ���� �ƴԿ��� �÷��̾� ���� �����ϴ� ������ ����Ǿ����ϴ�.");
            return;
        }
        battleState = BattleState.EnemyTurn;
        StartCoroutine(ExecuteEnemyTurn());
    }

    /// <summary>
    /// �� �� ���࿡ �ʿ��� ��ҵ� �ڷ�ƾ ����
    /// </summary>
    private IEnumerator ExecuteEnemyTurn()
    {
        // �� �� ����.
        Logger.Log("�� ����!");

        yield return null;
        // �� ���� �������� �÷��̾� ������ ����
        // ���� ���� ���� üũ(��� �� ���, ��� �÷��̾� ��� ��)
        if (CheckBattleEnd())
        {
            battleState = BattleState.BattleEnd;
            yield break;
        }

        battleState = BattleState.PlayerTurn;
        // ���� �÷��̾ �ൿ�� �� �ֵ��� UI Ȱ��ȭ ��
        yield break;
    }



    [Obsolete("��Ƽ�� �������ִ� ��ũ��Ʈ�� �ۼ��ؼ� �̸� ���������� ��������߸� ��.")]
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
