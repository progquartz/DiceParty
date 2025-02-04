using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseTarget : MonoBehaviour
{
    public event Action<BaseTarget> OnDead;
    public event Action<BaseTarget> OnRemoval;

    public BaseStat stat;

    public void Init()
    {
        stat.Hp = stat.maxHp;
        stat.Armour = 0;
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
