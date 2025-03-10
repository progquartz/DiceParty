using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{

    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
           
            float calcDamage = strength1 + caller.stat.StrengthStack;
            if (caller.stat.WeakenStack > 0) // ȣ���� ��� üũ
                calcDamage *= 0.75f;

           
            if (targetStat.WitherStack > 0) // Ÿ�� ���� üũ
                calcDamage = calcDamage * 1.25f;

            // ���� �������� 1���� ���� ���.
            int totalDamage = (int)calcDamage;

            // ������ �����ִٸ� �ƸӺ��� ���, ������ HP ���
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
            else // ����� �̸��� �ʴ� ���� ��, ���ð��� ������ ���.
            {
                if(targetStat.ThornStack > 0)
                {
                    caller.CalcEffect(EffectClassName.DebuffDamageEffect, targetStat.ThornStack);   
                }
            }
        }
    }
}
