using System;
using UnityEngine;

public class BattleManager : SingletonBehaviour<BattleManager>
{
    
    public void AmplifyEffect(BaseTarget[] targets, BaseEffect effect, int strength)
    {
        effect.Effect(targets, strength);
    }

    /// <summary>
    /// ���� Ÿ�� ���. �׾��� ���, �̿� ���� �ΰ� ��� ����.
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public BaseTarget DeathCheck()
    {
        return null;
    }

}
