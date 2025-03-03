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
    
    [SerializeField] private int initialDiceCount = 4; // 처음 주는 주사위 개수
    public int diceCountLimit = 8; // 주사위의 제한

    public int gold = 0;
    public float salesPercent = 0f;

    protected override void Init()
    {
        // 초기화 부분.
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

    /// Potion 부분
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


    /// Skill 부분
    
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

    /// Dice 부분



    private bool CheckInvenAvailability()
    {
        if (diceList.Count >= diceCountLimit)
        {
            // 더 이상 추가할 수 없음.
            // 교체하는 슬롯 필요.
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
            Logger.Log($"다이스의 인벤토리가 가득 차 교체를 실행합니다.");
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

        // Replace 될 수 있도록 조건 교체해주고...
        // 다이스에서 선택 콜이 들어오면... 고체.
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
