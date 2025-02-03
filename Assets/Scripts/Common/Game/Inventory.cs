using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : SingletonBehaviour<Inventory>
{
    [SerializeField] private List<Dice> diceList;
    [SerializeField] private DiceRoller diceRoller;

    [SerializeField] private List<PotionSlot> potionSlots;


    public int potionCountLimit = 3;
    public int diceCountLimit = 8;




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


    /// Dice 부분

    /// <summary>
    /// 새로운 주사위를 추가한다. 슬롯이 이미 가득 차 있을 경우, 교체를 위해서 교체 콜링을 필요로 한다.
    /// </summary>
    private void AddDiceToInventory(Dice newDice)
    {
        diceList.Add(newDice);
    }

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
            Logger.Log($"다이스의 인벤토리가 가득 차 교체를 실행합니다.");
            ReplaceDice();
            return false;
        }
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

    public List<Dice> GetAllDice()
    {
        return diceList;
    }


}
