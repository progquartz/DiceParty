using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

[CreateAssetMenu(fileName = "LootingCard", menuName = "Scriptable Objects/Looting/LootingCardDataSO")]
[System.Serializable]
public class LootingCardSO : ScriptableObject
{
    public int lootingStage;
    public List<LootingCardData> lootingCardData;

    public LootingCardData GetRandomLootSkill()
    {
        if(lootingCardData == null)
        {
            Logger.LogError("LootingCardData 내부의 데이터가 비었습니다.");
        }
        int randomIndex = Random.Range(0, lootingCardData.Count);
        return lootingCardData[randomIndex];
    }
}

[System.Serializable]
public class LootingCardData
{
    public int lootPower; // 1~100;
    public SkillDataSO skillDataSO;
}