using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseTarget : MonoBehaviour
{
    public event Action<BaseTarget> OnDead;

    public int Hp;
    public int maxHp;
    public int Armour;
    public int AdditionalDamageStack;
    public int AdditionalHealStack;

    public void Init()
    {
        Hp = maxHp;
        Armour = 0; 
    }


    // �׾��� ��� �̸� ó��.
    public void HandleDead()
    {
        Debug.Log($"{name}��(��) ����߽��ϴ�.");
        OnDead?.Invoke(this); // �̺�Ʈ ����
    }
}
