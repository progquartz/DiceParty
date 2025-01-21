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

            // ������ �����ִٸ� �ƸӺ��� ���, ������ HP ���
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
