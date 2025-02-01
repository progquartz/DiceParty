using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    [SerializeField] private List<Dice> diceList;
    [SerializeField] private DiceRoller diceRoller;

    public int diceCountLimit = 8;

    /// <summary>
    /// ���ο� �ֻ����� �߰��Ѵ�.
    /// </summary>
    public void AddDiceToInventory(DiceType diceType)
    {
        if(diceList.Count >= diceCountLimit)
        {
            // �� �̻� �߰��� �� ����.
            // ��ü�ϴ� ���� �ʿ�.
        }
        // ���� ������Ʈ ��ġ�� �ֻ����� �����Ѵٰ� ����
        Dice newDice = diceRoller.InstantiateNewDice(diceType);
        diceList.Add(newDice);
    }


    public List<Dice> GetAllDice()
    {
        return diceList;
    }

    public bool AddDice(DiceType diceType)
    {
        AddDiceToInventory (diceType);

    }
    public void AddD6()
    {
        AddDiceToInventory(DiceType.D6);
    }
}
