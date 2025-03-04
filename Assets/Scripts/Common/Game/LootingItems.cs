using UnityEngine;

[CreateAssetMenu(fileName = "LootingCard", menuName = "Scriptable Objects/Looting/LootingCardDataSO")]
[System.Serializable]
public class LootingCardSO : ScriptableObject
{
    public int lootingStage;
    public int lootPower; // 1~100;
    public SkillDataSO lootingSkillDataSO;
}

[CreateAssetMenu(fileName = "LootingPotion", menuName = "Scriptable Objects/Looting/LootingPotionDataSO")]
[System.Serializable]
public class LootingPotionSO : ScriptableObject
{
    public int lootingStage;
    public int lootPower;
    public PotionDataSO potionDataSO;
}

public class LootingTreasureSO : ScriptableObject
{
    public int lootingStage;
    public int lootPower;
}