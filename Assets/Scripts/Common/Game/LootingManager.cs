using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
    [SerializeField] private List<LootingPotionSO> lootingPotions;

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

        Logger.LogError("적절하지 않은 LootingTable이 설정되지 않았습니다.");
        return null;
    }

    public void OpenLootingTable(BattleType battleType, bool isBossStage)
    {
        LootingBattleType = battleType;
        UIManager.Instance.OpenUI<LootingUI>(new BaseUIData
        {
            ActionOnShow = () => { Debug.Log("전리품 UI 열림."); },
            ActionOnClose = () => 
            {
                LootingBattleType = BattleType.None;
                if(isBossStage)
                {
                    MapManager.Instance.GoToNextStage();
                }
                Debug.Log("전리품 UI 닫힘."); 
            }
        });
    }

    public LootingCardData GetRandomLootingCard()
    {
        
        int stageNum = MapManager.Instance.currentStageNum;
        foreach (var card in lootingCards)
        {
            if (card.lootingStage == stageNum)
            {
                return card.GetRandomLootSkill();
            }
        }
        Logger.LogError($"{stageNum}에 해당되는 LootingCardData가 없습니다.");
        return null;
    }

    public PotionDataSO GetRandomLootingPotion()
    {
        int stageNum = MapManager.Instance.currentStageNum;
        foreach (var potion in lootingPotions)
        { 
            if(potion.lootingStage == stageNum)
            {
                return potion.GetRandomPotionData();
            }
        }
        Logger.LogError($"{stageNum}에 해당되는 LootingPotionData가 없습니다.");
        return null;
    }



    
}

