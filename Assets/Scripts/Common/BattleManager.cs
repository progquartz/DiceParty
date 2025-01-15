using System;
using UnityEngine;

public class BattleManager : SingletonBehaviour<BattleManager>
{
    

    /// <summary>
    /// 죽은 타겟 계산. 죽었을 경우, 이에 따른 부가 요소 적립.
    /// </summary>
    /// <returns></returns>
    [Obsolete]
    public BaseTarget DeathCheck()
    {
        return null;
    }

}
