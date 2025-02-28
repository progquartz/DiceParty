using System.Collections.Generic;
using UnityEngine;

public class TargetSelf : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseTarget> targets = new List<BaseTarget>(); 
        targets.Add(caller);
        return targets;
    }
}
