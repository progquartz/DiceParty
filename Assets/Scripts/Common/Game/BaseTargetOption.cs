using System.Collections.Generic;
using UnityEngine;

public class BaseTargetOption : MonoBehaviour
{
    public virtual List<BaseTarget> GetTarget()
    {
        return null;
    }
}
