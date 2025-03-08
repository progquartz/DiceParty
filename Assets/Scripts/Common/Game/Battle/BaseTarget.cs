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
        stat.Armour = 0;
    }

    protected void EffectCalcOnTurnStart()
    {
        // ���� ȭ���� �� �����ϸ鼭 �ٷ� ���.
        CalcPoison();
        CalcFire();

        // ������ �����ִ� ��� �ش� �Ͽ� ���� ������ ������ ���ϰ� �����.
        CalcStun();
    }

    protected void EffectCalcOnTurnEnd()
    {
        // ���� ����� ���� ������ ����.
        CalcStrength();
        CalcWeaken();
    }

    private void CalcPoison()
    {
        if(stat.PoisonStack > 0)
        {
            SkillEffectData effData = new SkillEffectData();
            effData.targetClassName = TargetOptionClassName.TargetSelf;
            effData.effectClassName = EffectClassName.DamageEffect;
            effData.strength1 = stat.PoisonStack;
            BattleManager.Instance.SkillExecutor.UseSkill(effData);
            stat.PoisonStack--;
        }
    }

    private void CalcFire()
    {
        if (stat.FireStack > 0)
        {
            SkillEffectData effData = new SkillEffectData();
            effData.targetClassName = TargetOptionClassName.TargetSelf;
            effData.effectClassName = EffectClassName.DamageEffect;
            effData.strength1 = stat.FireStack;
            BattleManager.Instance.SkillExecutor.UseSkill(effData);
            stat.FireStack--;
        }
    }

    private void CalcStrength()
    {
        if(stat.StrengthStack > 0)
        {
            stat.StrengthStack--;
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
