using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    [SerializeField] private List<Dice> diceList;
    [SerializeField] private DiceRoller diceRoller;

    [SerializeField] private List<BasePotion> basePotion;


    public int potionCountLimit = 3;
    public int diceCountLimit = 8;

    /// <summary>
    /// ���ο� �ֻ����� �߰��Ѵ�. ������ �̹� ���� �� ���� ���, ��ü�� ���ؼ� ��ü �ݸ��� �ʿ�� �Ѵ�.
    /// </summary>
    private void AddDiceToInventory(Dice newDice)
    {
        diceList.Add(newDice);
    }


    public List<Dice> GetAllDice()
    {
        return diceList;
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
}
