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
    /// �� ���
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
    /// �� ���
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
        // �� ���� �� ��� �� ����.
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
