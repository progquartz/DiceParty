using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    [SerializeField] private List<Dice> diceList;
    [SerializeField] private DiceRoller diceRoller;

    public int diceCountLimit = 8;

    /// <summary>
    /// 새로운 주사위를 추가한다.
    /// </summary>
    public void AddDiceToInventory(DiceType diceType)
    {
        if(diceList.Count >= diceCountLimit)
        {
            // 더 이상 추가할 수 없음.
            // 교체하는 슬롯 필요.
        }
        // 현재 오브젝트 위치에 주사위를 생성한다고 가정
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
