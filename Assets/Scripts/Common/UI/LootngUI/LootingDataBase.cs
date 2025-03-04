using UnityEngine;

[System.Serializable]
public class LootingDataBase
{
    // ÀÏ¹Ý Looting È®·ü
    public LootingProbTable NormalBattleLoot;
    public LootingProbTable EliteBattleLoot;
    public LootingProbTable BossBattleLoot;
    public LootingProbTable TreasureLoot;

    public Sprite LootingCardImage;
}
