using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class BaseStat
{
    public int Hp;
    public int maxHp;
    public int ArmourStack;
    public bool isDead = false;

    // 효과 스택을 Dictionary로 관리
    private Dictionary<EffectKey, int> effectStacks = new Dictionary<EffectKey, int>();
    
    public event Action<EffectKey, int> OnUpdatingEffectStack;
    public event Action<EffectKey> OnRemovingEffectStack;

    public void CalcEffectStack(EffectKey effect, int value)
    {
        if(effectStacks.ContainsKey(effect))
        {
            if (effectStacks[effect] + value <= 0)
            {
                effectStacks.Remove(effect); // 0이면 제거
                OnRemovingEffectStack?.Invoke(effect);
            }
            else
            {
                effectStacks[effect] += value;
                OnUpdatingEffectStack?.Invoke(effect, effectStacks[effect]);
            }
        }
        else
        {
            effectStacks.Add(effect, value);
            OnUpdatingEffectStack?.Invoke(effect, effectStacks[effect]);
        }
    }

    public int GetEffectStack(EffectKey effect)
    {
        return effectStacks.ContainsKey(effect) ? effectStacks[effect] : 0;
    }

    public bool HasEffect(EffectKey effect)
    {
        if(effectStacks.ContainsKey(effect))
        {
            if (effectStacks[effect] > 0)
            {
                return true;
            }
            else
            {
                effectStacks.Remove(effect);
                OnRemovingEffectStack?.Invoke(effect);
            }
        }
        return false;
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
                Logger.LogError("버프나 / 디버프 등이 포함된 데이터를 복사하려 시도합니다.");
            }
        }
    }

    public void OnDead()
    {
        isDead = true;
        Hp = 0;
        ArmourStack = 0;

        RemoveAllEffects();
    }
    private void RemoveAllEffects()
    {
        List<EffectKey> keys = new List<EffectKey>(effectStacks.Keys);

        foreach(EffectKey key in keys)
        {
            Debug.Log($"사망에 의해 {key} 효과를 지웁니다.");
            effectStacks.Remove(key);
            OnRemovingEffectStack?.Invoke(key);
        }
    }

    public void Cleanse()
    {

    }
}
