using System;
using System.Security.Cryptography;
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
        // 독과 화염은 턴 시작하면서 바로 계산.
        CalcPoison();
        CalcFire();

        // 스턴은 남아있는 경우 해당 턴에 적의 패턴이 사용되지 못하게 만들기.
        CalcStun();
    }

    protected void EffectCalcOnTurnEnd()
    {
        // 힘과 쇠약은 턴이 끝나고 감소.
        CalcStrength();
        CalcWeaken();
    }

    private void CalcPoison()
    {
        if(stat.PoisonStack > 0)
        {
            SkillEffectData effData = new SkillEffectData();
            effData.targetClassName = TargetOptionClassName.TargetSelf;
            effData.effectClassName = EffectClassName.DamageEffect;
            effData.strength1 = stat.PoisonStack;
            BattleManager.Instance.SkillExecutor.UseSkill(effData);
            stat.PoisonStack--;
        }
    }

    private void CalcFire()
    {
        if (stat.FireStack > 0)
        {
            SkillEffectData effData = new SkillEffectData();
            effData.targetClassName = TargetOptionClassName.TargetSelf;
            effData.effectClassName = EffectClassName.DamageEffect;
            effData.strength1 = stat.FireStack;
            BattleManager.Instance.SkillExecutor.UseSkill(effData);
            stat.FireStack--;
        }
    }

    private void CalcStrength()
    {
        if(stat.StrengthStack > 0)
        {
            stat.StrengthStack--;
        }
    }
    
    private void CalcWeaken()
    {
        if(stat.WeakenStack > 0)
        {
            stat.WeakenStack--;
        }
    }

    private void CalcStun()
    {
        if(stat.StunnedStack > 0)
        {
            stat.StunnedStack--;
        }
    }

    public void HandleRevive()
    {
        Logger.Log($"{name} 캐릭터를 부활시킵니다.");
        stat.isDead = false;
        OnRevive?.Invoke(this);
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
