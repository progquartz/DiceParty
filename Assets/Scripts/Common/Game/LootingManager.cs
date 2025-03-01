using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum lootingType
{
    
}

[System.Serializable]
public class LootingData
{
    public int lootingStage;
    public int lootPower; // 1~100;
    public GameObject lootingItem;
}
public class LootingManager : SingletonBehaviour<LootingManager>
{
    public SkillUISpawner SkillUiSpawner;


    [SerializeField] private List<LootingData> lootingCards;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SkillUiSpawner = GetComponent<SkillUISpawner>();
    }

    
}

