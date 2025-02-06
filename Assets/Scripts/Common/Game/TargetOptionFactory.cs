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
        { "TargetAllCharacter", () => new TargetAllCharacter() },
        { "TargetRandomCharacter", () => new TargetRandomCharacter() },
        { "TargetAllEnemyDead",   () => new TargetAllEnemyDead() },
        { "TargetRandomEnemyDead", () => new TargetRandomEnemyDead() },
        { "TargetAllCharacterDead", () => new TargetAllCharacterDead() },
        { "TargetRandomCharacterDead", () => new TargetRandomCharacterDead() },
        { "TargetSelf", () => new TargetSelf() }
        // Ȯ��...
    };

    public static BaseTargetOption CreateTargetOption(TargetOptionClassName targetKey)
    {
        if (targetRegistry.TryGetValue(targetKey.ToString(), out var constructor))
        {
            return constructor();
        }

        Logger.LogWarning($"{targetKey} �� �̸��� ���� �ش��ϴ� TargetOption �����ڸ� ã�� �� �����ϴ�.");
        return null;
    }
}
