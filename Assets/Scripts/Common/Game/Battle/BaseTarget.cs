using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseTarget : MonoBehaviour
{
    public event Action<BaseTarget> OnDead;
    public event Action<BaseTarget> OnRemoval;
    public event Action<BaseTarget> OnRevive;

    public BaseStat stat;

    public void Init()
    {
        stat.Hp = stat.maxHp;
        stat.Armour = 0;
    }

    protected void EffectCalcOnTurnStart()
    {

    }

    protected void EffectCalcOnTurnEnd()
    {

    }

    public void HandleRevive()
    {
        Logger.Log($"{name} ĳ���͸� ��Ȱ��ŵ�ϴ�.");
        stat.isDead = false;
        OnRevive?.Invoke(this);
    }

    // �ܼ� ������ ��� ����(�� / ȭ�� ���...)�� �� ȿ���� ����.
    public void HandleDead()
    {
        Debug.Log($"{name}�� ����߽��ϴ�.");
        stat.isDead = true;
        OnDead?.Invoke(this); // �̺�Ʈ ����
    }

    // �� ������ �̺��� ���� ����, �� ������ ������ �������� �״´ٴ���, �����Ѵٴ��� ���� ȿ���� �� ȿ���� ����.
    public void HandleRemoval()
    {
        Debug.Log($"{name}�� ��Ʋ ����Ʈ���� �����Ǿ����ϴ�.");
        OnRemoval?.Invoke(this);
    }
}
