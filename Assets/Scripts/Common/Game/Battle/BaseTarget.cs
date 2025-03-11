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
        // 실질 데미지 관련
        CalcArmour();
        CalcPoison();
        CalcFire();

        CalcRegen();

        // 간접 버프
        CalcPassion();
        CalcWither();
        CalcThorn();


        // 정신계 버프 / 디버프
        CalcTaunt();
        CalcStun();
        
    }

    protected void EffectCalcOnTurnEnd()
    {
        CalcConfuse(); // 혼란은 턴이 끝나고 감소.
        CalcWeaken(); // 쇠약은 턴이 끝나고 감소.

        CalcFortify(); // 방벽(보호막 추가)
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
        BattleManager.Instance.SkillExecutor.UseSkill(effData);
    }
    /// <summary>
    /// 독 계산
    /// </summary>
    private void CalcPoison()
    {
        if(stat.HasEffect(EffectKey.PoisonEffect))
        {
            CalcEffect(EffectKey.DebuffDamageEffect, stat.GetEffect(EffectKey.PoisonEffect));
            stat.CalcEffectStack(EffectKey.PoisonEffect, -1);
        }
    }

    /// <summary>
    /// 불 계산
    /// </summary>
    private void CalcFire()
    {
        if(stat.HasEffect(EffectKey.FireEffect))
        {
            CalcEffect(EffectKey.DebuffDamageEffect, stat.GetEffect(EffectKey.FireEffect));
            stat.CalcEffectStack(EffectKey.FireEffect, -1);
        }
    }

    private void CalcRegen()
    {
        if(stat.HasEffect(EffectKey.RegenEffect))
        {
            CalcEffect(EffectKey.HealEffect, stat.GetEffect(EffectKey.RegenEffect));
            stat.CalcEffectStack(EffectKey.RegenEffect, -1);
        }
    }

    private void CalcArmour()
    {
        // 턴 시작 시 모든 방어막 삭제.
        stat.ArmourStack = 0;
    }

    private void CalcFortify()
    {
        if(stat.HasEffect(EffectKey.FortifyEffect))
        {
            CalcEffect(EffectKey.ArmourEffect, stat.GetEffect(EffectKey.FortifyEffect));
            stat.CalcEffectStack(EffectKey.FortifyEffect, -1);
        }
    }

    private void CalcPassion()
    {
        if(stat.HasEffect(EffectKey.PassionEffect))
        {
            CalcEffect(EffectKey.StrengthEffect, stat.GetEffect(EffectKey.PassionEffect));
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
