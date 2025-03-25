using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LootingGold", menuName = "Scriptable Objects/Looting/LootingGoldDataSO")]
[System.Serializable]
public class LootingGoldSO : ScriptableObject
{
    public List<LootingGoldData> lootingGoldDatas;
}

[System.Serializable]
public class LootingGoldData
{
    public int LootingStage;
    public int MinValue;
    public int MaxValue;
    public int EliteAdditionalGold;
    public int BossAdditionalGold;

    public int GetGoldValue(BattleType battleType)
    {
        int battleInt = (int)battleType % 10;
        int baseLootGold = Random.Range(MinValue, MaxValue);
        if(battleInt == 0)
        {
            return baseLootGold;
        }
        else if (battleInt == 1)
        {
            return baseLootGold + EliteAdditionalGold;
        }
        else if (battleInt == 2)
        {
            return baseLootGold + BossAdditionalGold;
        }
        return -1;
    }
}
