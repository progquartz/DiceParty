using UnityEngine;

[System.Serializable]
public class LootingDataBase
{
    // 일반 Looting 확률
    public LootingProbTable NormalBattleLoot;
    public LootingProbTable EliteBattleLoot;
    public LootingProbTable BossBattleLoot;
    public LootingProbTable TreasureLoot;

    public Sprite LootingCardImage;
    public Sprite LootingGoldImage;
}
