using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;



public class LootingManager : SingletonBehaviour<LootingManager>
{
    public SkillUISpawner SkillUiSpawner;


    [SerializeField] private List<LootingCard> lootingCards;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SkillUiSpawner = GetComponent<SkillUISpawner>();
    }

    public LootingCard GetRandomLootingCard(int stageNum)
    {
        List<LootingCard> lootingCardList = new List<LootingCard>();
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

