

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
            Logger.LogError("LootingPotionData 내부의 데이터가 비었습니다.");
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