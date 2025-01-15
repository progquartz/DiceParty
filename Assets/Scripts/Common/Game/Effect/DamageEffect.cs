using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{

    public override void Effect(List<BaseTarget> targets, int strength)
    {
        foreach (var target in targets)
        {
            int totalDamage = strength + target.AdditionalDamageStack;

            // 방어력이 남아있다면 아머부터 깎고, 남으면 HP 깎기
            if (target.Armour > 0)
            {
                target.Armour -= totalDamage;
                if (target.Armour < 0)
                {
                    int hpDamage = -target.Armour;
                    target.Armour = 0;
                    target.Hp -= hpDamage;
                }
            }
            else
            {
                target.Hp -= totalDamage;
            }

            
            if (target.Hp <= 0)
            {
                target.Hp = 0;
                target.HandleDead();
            }
        }
    }
}
