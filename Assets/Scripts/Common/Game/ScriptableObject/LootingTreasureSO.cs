using UnityEngine;


public class LootingTreasureSO : ScriptableObject
{
    public int lootingStage;
    public int lootPower;
}

[System.Serializable]
public class LootingTreasureData
{
    public int lootPower; // 1~100;
}