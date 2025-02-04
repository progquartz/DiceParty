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


    // 단순 데미지 기반 죽음(독 / 화염 등등...)은 이 효과를 적용.
    public void HandleDead()
    {
        Debug.Log($"{name}이 사망했습니다.");
        stat.isDead = true;
        OnDead?.Invoke(this); // 이벤트 발행
    }

    // 적 보스와 쫄병이 있을 때에, 적 보스가 죽으면 나머지가 죽는다던가, 자폭한다던가 등의 효과는 이 효과를 적용.
    public void HandleRemoval()
    {
        Debug.Log($"{name}이 배틀 리스트에서 삭제되었습니다.");
        OnRemoval?.Invoke(this);
    }
}
