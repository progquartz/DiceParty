

using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootingPotion", menuName = "Scriptable Objects/Looting/LootingPotionDataSO")]
[System.Serializable]
public class LootingPotionSO : ScriptableObject
{
    public int lootingStage;
    public List<LootingPotionData> lootingPotionData;

    public PotionDataSO GetRandomPotionData()
    {
        if(lootingPotionData == null)
        {
            Logger.LogError("LootingPotionData ������ �����Ͱ� ������ϴ�.");
        }
        int randomIndex = Random.Range(0, lootingPotionData.Count);
        return lootingPotionData[randomIndex].skillDataSO;
    }
}

[System.Serializable]
public class LootingPotionData
{
    public int lootPower; // 1~100;
    public PotionDataSO skillDataSO;
}