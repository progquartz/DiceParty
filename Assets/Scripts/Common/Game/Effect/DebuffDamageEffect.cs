using System.Collections.Generic;
using UnityEngine;

public class DebuffDamageEffect : BaseEffect
{

    public override void Effect(List<BaseTarget> targets, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int totalDamage = strength1;

            // 방어력이 남아있다면 아머부터 깎고, 남으면 HP 깎기
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
        }
    }
}
