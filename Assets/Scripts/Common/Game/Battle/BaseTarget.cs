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
        // ���� ������ ����
        CalcArmour();
        CalcPoison();
        CalcFire();

        CalcRegen();

        // ���� ����
        CalcPassion();
        CalcWither();
        CalcThorn();


        // ���Ű� ���� / �����
        CalcTaunt();
        CalcStun();
        
    }

    protected void EffectCalcOnTurnEnd()
    {
        CalcConfuse(); // ȥ���� ���� ������ ����.
        CalcWeaken(); // ����� ���� ������ ����.

        CalcFortify(); // �溮(��ȣ�� �߰�)
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
    /// �� ���
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
    /// �� ���
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
        // �� ���� �� ��� �� ����.
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
            // Confuse ȿ���� ���� ȿ�� �ߵ�.
            stat.ConfuseStack--;
        }
    }

    private void CalcTaunt()
    {
        if(stat.TauntStack > 0)
        {
            // Taunt ȿ�� �ߵ� ó��.
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
        Logger.Log($"{name} ĳ���͸� ��Ȱ��ŵ�ϴ�.");
        stat.isDead = false;
        OnRevive?.Invoke(this);
    }

    // �ܼ� ������ ��� ����(�� / ȭ�� ���...)�� �� ȿ���� ����.
    public void HandleDead()
    {
        Debug.Log($"{name}�� ����߽��ϴ�.");
        stat.isDead = true;
        OnDead?.Invoke(this); // �̺�Ʈ ����
    }

    // �� ������ �̺��� ���� ����, �� ������ ������ �������� �״´ٴ���, �����Ѵٴ��� ���� ȿ���� �� ȿ���� ����.
    public void HandleRemoval()
    {
        Debug.Log($"{name}�� ��Ʋ ����Ʈ���� �����Ǿ����ϴ�.");
        OnRemoval?.Invoke(this);
    }
}
