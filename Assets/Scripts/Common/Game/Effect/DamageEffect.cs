using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{

    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
           
            float calcDamage = strength1 + caller.stat.GetEffect(EffectKey.StrengthEffect);
            if (caller.stat.HasEffect(EffectKey.WeakenEffect)) // 호출자 약화 체크
                calcDamage *= 0.75f;

           
            if (targetStat.HasEffect(EffectKey.WitherEffect)) // 타겟 시듦 체크
                calcDamage = calcDamage * 1.25f;

            // 최종 데미지는 1보다 작지 않음.
            int totalDamage = (int)calcDamage;

            // 방어도가 남아있다면 아머부터 깎고, 나머지 HP 깎음
            if (targetStat.ArmourStack > 0)
            {
                targetStat.ArmourStack -= totalDamage;
                if (targetStat.ArmourStack < 0)
                {
                    int hpDamage = -targetStat.ArmourStack;
                    targetStat.ArmourStack = 0;
                    targetStat.Hp -= hpDamage;
                }
            }
            else
            {
                targetStat.Hp -= totalDamage;
            }

            
            if (targetStat.Hp <= 0)
            {
                targetStat.Hp = 0;
                target.HandleDead();
            }
            else // 데미지 이후에 죽지 않은 경우 중, 가시가시 데미지 계산.
            {
                if(targetStat.HasEffect(EffectKey.ThornEffect))
                {
                    caller.CalcEffect(EffectKey.DebuffDamageEffect, targetStat.GetEffect(EffectKey.ThornEffect));
                }
            }
        }
    }
}
