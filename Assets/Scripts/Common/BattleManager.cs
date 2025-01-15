using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : SingletonBehaviour<BattleManager>
{

    // ������ ���� ���� Ÿ�ٵ� ��� (��, �Ʊ� ���)
    private List<BaseTarget> activeTargets = new List<BaseTarget>();

    public void RegisterTarget(BaseTarget target)
    {
        if (!activeTargets.Contains(target))
        {
            activeTargets.Add(target);
            target.OnDead += OnTargetDead; // ��� �̺�Ʈ ����
        }
    }

    private void OnTargetDead(BaseTarget deadTarget)
    {
        // ���� �������� ���� ����� �����ϰų�, UI ���� ��
        Debug.Log($"BattleManager: {deadTarget.name} ��� ó��");

        if (activeTargets.Contains(deadTarget))
        {
            activeTargets.Remove(deadTarget);
        }

        // ���� ��/�Ʊ� üũ �� ���� �¸�/�й� ���� ��
        if (CheckAllEnemiesDead())
        {
            Debug.Log("�÷��̾� �¸�!");
        }
    }

    private bool CheckAllEnemiesDead()
    {
        foreach (var t in activeTargets)
        {
            // BaseEnemy�� ��ӹ��� ���� ��� ������ false
            if (t is BaseEnemy && t.Hp > 0)
                return false;
        }
        return true;
    }

}
