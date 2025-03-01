using UnityEngine;

[CreateAssetMenu(fileName = "LootingCard", menuName = "Scriptable Objects/Looting/LootingCardDataSO")]
[System.Serializable]
public class LootingCard : ScriptableObject
{
    public int lootingStage;
    public int lootPower; // 1~100;
    public SkillDataSO lootingSkillDataSO;
}

[CreateAssetMenu(fileName = "LootingPotion", menuName = "Scriptable Objects/Looting/LootingPotionDataSO")]
[System.Serializable]
public class LootingPotion : ScriptableObject
{
    public int lootingStage;
    public int lootPower;
    public PotionDataSO potionDataSO;
}

public class LootingTreature : ScriptableObject
{
    public int lootingStage;
    public int lootPower;
}