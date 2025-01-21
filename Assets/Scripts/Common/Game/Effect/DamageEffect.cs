using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{

    public override void Effect(List<BaseTarget> targets, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int totalDamage = strength1 + targetStat.AdditionalDamageStack;

            // 방어력이 남아있다면 아머부터 깎고, 남으면 HP 깎기
            if (targetStat.Armour > 0)
            {
                targetStat.Armour -= totalDamage;
                if (targetStat.Armour < 0)
                {
                    int hpDamage = -targetStat.Armour;
                    targetStat.Armour = 0;
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
