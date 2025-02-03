using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    [SerializeField] private List<Dice> diceList;
    [SerializeField] private DiceRoller diceRoller;

    [Header("���� ����")]
    [SerializeField] private List<PotionSlot> potionSlots;
    [Header("��ų ����")]
    [SerializeField] private List<SkillUISlot> skillUISlots;


    public int potionCountLimit = 3;
    public int diceCountLimit = 8;




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
    private void AddDiceToInventory(Dice newDice)
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

    private Dice InstantiateNewDice(DiceType diceType)
    {
        Dice newDice = diceRoller.InstantiateNewDice(diceType);
        return newDice;
    }

    public bool AddDice(DiceType diceType)
    {
        if(CheckInvenAvailability())
        {
            Dice newDice = InstantiateNewDice(diceType);
            AddDiceToInventory(newDice);
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

    public List<Dice> GetAllDice()
    {
        return diceList;
    }


}
