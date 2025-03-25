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
        // 확장...
    };

    public static BaseTargetOption CreateTargetOption(TargetOptionClassName targetKey)
    {
        if (targetRegistry.TryGetValue(targetKey.ToString(), out var constructor))
        {
            return constructor();
        }

        Logger.LogWarning($"{targetKey}란 이름을 가진 해당하는 TargetOption 생성자를 찾을 수 없습니다.");
        return null;
    }
}
