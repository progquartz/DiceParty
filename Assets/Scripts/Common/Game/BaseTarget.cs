using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseTarget : MonoBehaviour
{
    public event Action<BaseTarget> OnDead;

    public BaseStat stat;

    public void Init()
    {
        stat.Hp = stat.maxHp;
        stat.Armour = 0;
    }


    // �׾��� ��� �̸� ó��.
    public void HandleDead()
    {
        Debug.Log($"{name}��(��) ����߽��ϴ�.");
        OnDead?.Invoke(this); // �̺�Ʈ ����
    }
}
