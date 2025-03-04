using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootingProbTable
{
    public int CardPercent;
    public int TreasurePercent;
    public int PotionPercent;
    public int DicePercent;
}
public class LootingManager : SingletonBehaviour<LootingManager>
{
    public SkillUISpawner SkillUiSpawner;




    public BattleType LootingBattleType;

    [SerializeField] private List<LootingCardSO> lootingCards;

    public LootingDataBase LootingDataBase = new LootingDataBase();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SkillUiSpawner = GetComponent<SkillUISpawner>();
    }

    public LootingProbTable GetLootingTable()
    {
        switch((int)LootingBattleType % 10)
        {
            case 1:
                return LootingDataBase.NormalBattleLoot;
            case 2:
                return LootingDataBase.EliteBattleLoot;
            case 3:
                return LootingDataBase.BossBattleLoot;
            case 0:
                return LootingDataBase.TreasureLoot;
        }

        Logger.LogError("정상적인 LootingTable이 발행되지 않았습니다.");
        return null;
    }

    public void OpenLootingTable(BattleType battleType)
    {
        LootingBattleType = battleType;
        UIManager.Instance.OpenUI<LootingUI>(new BaseUIData
        {
            ActionOnShow = () => { Debug.Log("루팅 UI 열림."); },
            ActionOnClose = () => 
            {
                LootingBattleType = BattleType.None;
                Debug.Log("루팅 UI 닫힘."); 
            }
        });
    }

    public LootingCardSO GetRandomLootingCard(int stageNum)
    {
        List<LootingCardSO> lootingCardList = new List<LootingCardSO>();
        foreach(var card in lootingCards) 
        {
            if(card.lootingStage == stageNum)
            {
                lootingCardList.Add(card);
            }
        }

        int randomIndex = Random.Range(0, lootingCardList.Count);
        return lootingCardList[randomIndex];
    }



    
}

