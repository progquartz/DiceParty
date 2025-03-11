using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public int Hp;
    public int maxHp;
    public int ArmourStack;
    public bool isDead = false;

    // ȿ�� ������ Dictionary�� ����
    private Dictionary<EffectKey, int> effectStacks = new Dictionary<EffectKey, int>();

    public void SetEffectStack(EffectKey effect, int value)
    {
        if (value > 0)
        {
            effectStacks[effect] = value;
        }
        else if (effectStacks.ContainsKey(effect))
        {
            effectStacks.Remove(effect); // 0�̸� ����
        }
    }

    public void CalcEffectStack(EffectKey effect, int value)
    {
        if(effectStacks.ContainsKey(effect))
        {
            if (effectStacks[effect] + value <= 0)
            {
                effectStacks.Remove(effect); // 0�̸� ����
            }
            else
            {
                effectStacks[effect] += value;
            }
        }
        else
        {
            effectStacks.Add(effect, value);
        }
    }

    public int GetEffect(EffectKey effect)
    {
        return effectStacks.ContainsKey(effect) ? effectStacks[effect] : 0;
    }

    public bool HasEffect(EffectKey effect)
    {
        return effectStacks.ContainsKey(effect);
    }

    public Dictionary<EffectKey, int> GetAllEffects()
    {
        return new Dictionary<EffectKey, int>(effectStacks);
    }

    public void SetStat(BaseStat stat)
    {
        Hp = stat.Hp;
        maxHp = stat.maxHp;
        ArmourStack = stat.ArmourStack;
        isDead = stat.isDead;

        var originalEffects = stat.GetAllEffects();
        if(originalEffects != null)
        {
            if(originalEffects.Count > 0)
            {
                Logger.LogError("����� / ���� ���� ���Ե� ������ ������ �ε�Ƿ��� �մϴ�.");
            }
        }
    }

    public void Cleanse()
    {

    }
}
