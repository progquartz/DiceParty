using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    public Transform cardParent;

    [SerializeField] private List<DiceType> diceList;

    [Header("���� ����")]
    [SerializeField] private List<PotionSlot> potionSlots;
    public int potionCountLimit = 3;

    [Header("��ų ����")]
    [SerializeField] private List<SkillUISlot> skillUISlots;


    [SerializeField] private List<SkillDataSO> initializeSkillDataSO;
    
    [SerializeField] private int initialDiceCount = 4; // ó�� �ִ� �ֻ��� ����
    public int diceCountLimit = 8; // �ֻ����� ����

    public int gold = 0;
    public float salesPercent = 0f;

    protected override void Init()
    {
        // �ʱ�ȭ �κ�.
        InitializingInventory();
        BattleManager.Instance.ResetDiceToDummy();
    }

    private void InitializingInventory()
    {
        // �ֻ��� �߰�
        for(int i = 0; i < initialDiceCount; i++)
        {
            AddDice(DiceType.D6);
        }
        // ��ų �߰�
        foreach(SkillDataSO skillData in initializeSkillDataSO)
        {
            AddNewSkillInSlot(skillData);
        }


    }

    /// Potion �κ�
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


    /// Skill �κ�
    
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

    /// Dice �κ�



    private bool CheckInvenAvailability()
    {
        if (diceList.Count >= diceCountLimit)
        {
            // �� �̻� �߰��� �� ����.
            // ��ü�ϴ� ���� �ʿ�.
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
            Logger.Log($"���̽��� �κ��丮�� ���� �� ��ü�� �����մϴ�.");
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

        // Replace �� �� �ֵ��� ���� ��ü���ְ�...
        // ���̽����� ���� ���� ������... ��ü.
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
