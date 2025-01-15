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
        // Ȯ��...
    };

    public static BaseTargetOption CreateTargetOption(string targetKey)
    {
        if (targetRegistry.TryGetValue(targetKey, out var constructor))
        {
            return constructor();
        }

        Logger.LogWarning($"{targetKey} �� �ش��ϴ� TargetOption �����ڸ� ã�� �� �����ϴ�.");
        return null;
    }
}
