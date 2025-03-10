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

    public void CalcEffect(EffectClassName effectClass, int strength1, int strength2 = 0)
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
        if(stat.PoisonStack > 0)
        {
            CalcEffect(EffectClassName.DebuffDamageEffect, stat.PoisonStack);
            stat.PoisonStack--;
        }
    }

    /// <summary>
    /// 불 계산
    /// </summary>
    private void CalcFire()
    {
        if (stat.FireStack > 0)
        {
            CalcEffect(EffectClassName.DebuffDamageEffect, stat.FireStack); 
            stat.FireStack--;
        }
    }

    private void CalcRegen()
    {
        if(stat.RegenStack > 0)
        {
            CalcEffect(EffectClassName.HealEffect, stat.RegenStack);
            stat.RegenStack--;
        }
    }

    private void CalcArmour()
    {
        // 턴 시작 시 모든 방어막 삭제.
        stat.ArmourStack = 0;
    }

    private void CalcFortify()
    {
        if(stat.fortifyStack > 0)
        {
            CalcEffect(EffectClassName.ArmourEffect, stat.fortifyStack);
            stat.fortifyStack--;
        }
    }

    private void CalcPassion()
    {
        if(stat.PassionStack > 0)
        {
            CalcEffect(EffectClassName.StrengthEffect, stat.PassionStack);
            stat.PassionStack--;
        }
    }

    private void CalcConfuse()
    {
        if (stat.ConfuseStack > 0)
        {
            // Confuse 효과를 내는 효과 발동.
            stat.ConfuseStack--;
        }
    }

    private void CalcTaunt()
    {
        if(stat.TauntStack > 0)
        {
            // Taunt 효과 발동 처리.
           stat.TauntStack--;
        }
    }

    private void CalcThorn()
    {
        if(stat.ThornStack > 0)
        {
            stat.ThornStack--;
        }
    }

    private void CalcWither()
    {
        if(stat.WitherStack > 0)
        {
            stat.WitherStack--;
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
