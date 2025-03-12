using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    public Transform cardParent;

    [SerializeField] private List<DiceType> diceList;

    [Header("포션 슬롯")]
    [SerializeField] private List<PotionSlot> potionSlots;
    public int potionCountLimit = 3;

    [Header("스킬 슬롯")]
    [SerializeField] private List<SkillUISlot> skillUISlots;


    [SerializeField] private List<SkillDataSO> initializeSkillDataSO;
    
    [SerializeField] private int initialDiceCount = 4; // 처음 가진 주사위 개수
    public int diceCountLimit = 8; // 주사위 최대 개수

    public int gold = 0;
    public float salesPercent = 0f;

    protected override void Init()
    {
        // 초기화 작업.
        InitializingInventory();
        BattleManager.Instance.ResetDiceToDummy();
    }

    private void InitializingInventory()
    {
        // 주사위 추가
        for(int i = 0; i < initialDiceCount; i++)
        {
            AddDice(DiceType.D6);
        }
        // 스킬 추가
        foreach(SkillDataSO skillData in initializeSkillDataSO)
        {
            AddNewSkillInSlot(skillData);
        }
    }

    /// 포션 사용
    public void UsePotionInSlots(int index)
    {
        potionSlots[index].UsePotion();
    }
    public PotionDataSO GetPotionData(int index)
    {
        return potionSlots[index].GetPotionData();
    }

    public void EmptyPotionSlot(int index)
    {
        potionSlots[index].EmptyPotion();
    }

    public void PayPrice(int amount)
    {
        if(gold - amount >= 0) 
        {
            gold -= amount; 
        }
    }


    /// 스킬 사용
    
    public List<SkillUI> GetAllUsingSkill()
    {
        List<SkillUI> skillList = new List<SkillUI>();
        foreach (var skill in skillUISlots)
        {
            if(skill.HasSkill())
            {
                skillList.Add(skill.GetSkill());
            }
        }
        return skillList;
    }

    public bool AddNewSkillInSlot(SkillDataSO skillDataSO)
    {
        foreach (var slot in skillUISlots)
        {
            if(slot.GetCharacter().CharacterType == skillDataSO.CharacterType)
            {
                bool isAttached = slot.AttachNewSkillUI(skillDataSO);
                if (isAttached)
                    return true;
            }
        }
        return false;
    }

    /// 주사위 사용



    private bool CheckInvenAvailability()
    {
        if (diceList.Count >= diceCountLimit)
        {
            // 주사위 추가 불가능.
            return false;
        }
        else
        {
            return true;
        }
    }

    public List<DiceType> GetDiceList()
    {
        return diceList;
    }


    public bool AddDice(DiceType diceType)
    {
        if(CheckInvenAvailability())
        {
            AddDiceToInventory(diceType);
            return true;
        }
        else
        {
            Logger.Log($"주사위 추가 불가능. 주사위 교체 작업 진행.");
            ReplaceDice();
            return false;
        }
    }

    private void AddDiceToInventory(DiceType newDice)
    {
        diceList.Add(newDice);
    }

    public void ReplaceDice()
    {
        throw new System.NotImplementedException();

        // 주사위 교체 작업 중...
        // 주사위 교체 작업 중...
    }
    public void AddD6()
    {
        AddDice(DiceType.D6);
    }

    public List<DiceType> GetAllDice()
    {
        return diceList;
    }


}
