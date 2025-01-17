using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetAllEnemy : BaseTargetOption
{
    public override List<BaseTarget> GetTarget()
    {
        
        return FindAllEnemy();
    }

    [Obsolete("모든 적 찾아오는 기법 완성안됨.")]
    private List<BaseTarget> FindAllEnemy()
    {
        // 예: 씬 상의 모든 BaseEnemy를 찾기
        BaseEnemy[] enemies = GameObject.FindObjectsOfType<BaseEnemy>();
        return new List<BaseTarget>(enemies);
    }
}
