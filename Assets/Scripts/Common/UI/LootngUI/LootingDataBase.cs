using UnityEngine;

[System.Serializable]
public class LootingDataBase
{
    // �Ϲ� Looting Ȯ��
    public LootingProbTable NormalBattleLoot;
    public LootingProbTable EliteBattleLoot;
    public LootingProbTable BossBattleLoot;
    public LootingProbTable TreasureLoot;

    public Sprite LootingCardImage;
}
