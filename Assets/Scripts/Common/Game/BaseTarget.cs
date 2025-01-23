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


    // 죽었을 경우 이를 처리.
    public void HandleDead()
    {
        Debug.Log($"{name}이 사망했습니다.");
        stat.isDead = true;
        OnDead?.Invoke(this); // 이벤트 발행
    }

    public void HandleRemoval()
    {
        Debug.Log($"{name}이 배틀 리스트에서 삭제되었습니다.");
        OnRemoval?.Invoke(this);
    }
}
