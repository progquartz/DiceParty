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


    // 죽었을 경우 이를 처리.
    public void HandleDead()
    {
        Debug.Log($"{name}이(가) 사망했습니다.");
        OnDead?.Invoke(this); // 이벤트 발행
    }
}
