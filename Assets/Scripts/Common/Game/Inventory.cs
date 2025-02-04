using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    [SerializeField] private List<DiceType> diceList;

    [Header("���� ����")]
    [SerializeField] private List<PotionSlot> potionSlots;
    [Header("��ų ����")]
    [SerializeField] private List<SkillUISlot> skillUISlots;


    public int potionCountLimit = 3;
    public int diceCountLimit = 8;


    protected override void Init()
    {
        // �ʱ�ȭ �κ�.
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

    /// Dice �κ�

    /// <summary>
    /// ���ο� �ֻ����� �߰��Ѵ�. ������ �̹� ���� �� ���� ���, ��ü�� ���ؼ� ��ü �ݸ��� �ʿ�� �Ѵ�.
    /// </summary>
    private void AddDiceToInventory(DiceType newDice)
    {
        diceList.Add(newDice);
    }

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
