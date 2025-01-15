using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{

    public override void Effect(List<BaseTarget> targets, int strength)
    {
        foreach (var target in targets)
        {
            int totalDamage = strength + target.AdditionalDamageStack;

            // ������ �����ִٸ� �ƸӺ��� ���, ������ HP ���
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
