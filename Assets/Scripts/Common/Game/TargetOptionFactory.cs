using System;
using System.Collections.Generic;
using UnityEngine;

public static class TargetOptionFactory
{
    private static Dictionary<string, Func<BaseTargetOption>> targetRegistry
        = new Dictionary<string, Func<BaseTargetOption>>()
    {
        { "TargetAllEnemy",   () => new TargetAllEnemy() },
        { "TargetRandomEnemy", () => new TargetRandomEnemy() },
        // 확장...
    };

    public static BaseTargetOption CreateTargetOption(string targetKey)
    {
        if (targetRegistry.TryGetValue(targetKey, out var constructor))
        {
            return constructor();
        }

        Logger.LogWarning($"{targetKey} 에 해당하는 TargetOption 생성자를 찾을 수 없습니다.");
        return null;
    }
}
