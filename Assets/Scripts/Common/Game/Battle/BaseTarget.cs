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
        stat.ArmourStack = 0;
    }

    protected void EffectCalcOnTurnStart()
    {
        // 디버프 데미지 계산
        CalcArmour();
        CalcPoison();
        CalcFire();

        CalcRegen();

        // 버프 계산
        CalcPassion();
        CalcWither();
        CalcThorn();


        // 행동에 관한 / 제어기
        CalcTaunt();
        CalcStun();
        
    }

    protected void EffectCalcOnTurnEnd()
    {
        CalcConfuse(); // 혼란은 턴이 끝나면 감소.
        CalcWeaken(); // 약화는 턴이 끝나면 감소.

        CalcFortify(); // 요새(보호막 추가)
    }

    public void CalcEffect(EffectKey effectClass, int strength1, int strength2 = 0)
    {
        SkillEffectData effData = new SkillEffectData();
        effData.targetClassName = TargetOptionClassName.TargetSelf;
        effData.effectClassName = effectClass;
        effData.strength1 = strength1;
        if(strength2 != 0)
        {
            effData.strength2 = strength2;
        }
        else
        {
            effData.strength2 = 0;
        }
        BattleManager.Instance.SkillExecutor.UseSkill(effData, this);
    }
    /// <summary>
    /// 독 계산
    /// </summary>
    private void CalcPoison()
    {
        if(stat.HasEffect(EffectKey.PoisonEffect))
        {
            CalcEffect(EffectKey.DebuffDamageEffect, stat.GetEffectStack(EffectKey.PoisonEffect));
            Debug.Log($"Poison Effect를 {stat.GetEffectStack(EffectKey.PoisonEffect)} 에서 -1합니다.");
            stat.CalcEffectStack(EffectKey.PoisonEffect, -1);
        }
    }

    /// <summary>
    /// 화염 계산
    /// </summary>
    private void CalcFire()
    {
        if(stat.HasEffect(EffectKey.FireEffect))
        {
            CalcEffect(EffectKey.DebuffDamageEffect, stat.GetEffectStack(EffectKey.FireEffect));
            Debug.Log($"Fire Effect를 {stat.GetEffectStack(EffectKey.FireEffect)} 에서 -1합니다.");
            stat.CalcEffectStack(EffectKey.FireEffect, -1);
        }
    }

    private void CalcRegen()
    {
        if(stat.HasEffect(EffectKey.RegenEffect))
        {
            CalcEffect(EffectKey.HealEffect, stat.GetEffectStack(EffectKey.RegenEffect));
            stat.CalcEffectStack(EffectKey.RegenEffect, -1);
        }
    }

    private void CalcArmour()
    {
        // 턴 시작 시 방어도 초기화.
        stat.ArmourStack = 0;
    }

    private void CalcFortify()
    {
        if(stat.HasEffect(EffectKey.FortifyEffect))
        {
            CalcEffect(EffectKey.ArmourEffect, stat.GetEffectStack(EffectKey.FortifyEffect));
            stat.CalcEffectStack(EffectKey.FortifyEffect, -1);
        }
    }

    private void CalcPassion()
    {
        if(stat.HasEffect(EffectKey.PassionEffect))
        {
            CalcEffect(EffectKey.StrengthEffect, stat.GetEffectStack(EffectKey.PassionEffect));
            stat.CalcEffectStack(EffectKey.PassionEffect, -1);
        }
    }

    private void CalcConfuse()
    {
        if(stat.HasEffect(EffectKey.ConfuseEffect))
        {
            stat.CalcEffectStack(EffectKey.ConfuseEffect, -1);
        }
    }

    private void CalcTaunt()
    {
        if(stat.HasEffect(EffectKey.TauntEffect))
        {
            stat.CalcEffectStack(EffectKey.TauntEffect, -1);
        }
    }

    private void CalcThorn()
    {
        if(stat.HasEffect(EffectKey.ThornEffect))
        {
            stat.CalcEffectStack(EffectKey.ThornEffect, -1);
        }
    }

    private void CalcWither()
    {
        if(stat.HasEffect(EffectKey.WitherEffect))
        {
            stat.CalcEffectStack(EffectKey.WitherEffect, -1);
        }
    }
    
    private void CalcWeaken()
    {
        if(stat.HasEffect(EffectKey.WeakenEffect))
        {
            stat.CalcEffectStack(EffectKey.WeakenEffect, -1);
        }
    }

    private void CalcStun()
    {
        if(stat.HasEffect(EffectKey.StunEffect))
        {
            stat.CalcEffectStack(EffectKey.StunEffect, -1);
        }
    }

    public void HandleRevive()
    {
        Logger.Log($"{name} 캐릭터를 부활시킵니다.");
        stat.isDead = false;
        OnRevive?.Invoke(this);
    }

    // 단순 사망시 발생 이벤트(독 / 화염 등...)에 의한 효과를 위함.
    public void HandleDead()
    {
        Debug.Log($"{name}이 사망했습니다.");
        stat.OnDead();
        OnDead?.Invoke(this); // 이벤트 발생
    }

    // 이 이벤트는 이벤트 발생 순서, 즉 사망시 이벤트가 발생하고 죽는다던가, 제거한다던가 하는 효과를 위한 효과를 위함.
    public void HandleRemoval()
    {
        Debug.Log($"{name}이 배틀 필드에서 제거되었습니다.");
        OnRemoval?.Invoke(this);
    }
}
