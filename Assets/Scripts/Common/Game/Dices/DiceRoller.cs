using UnityEngine;
using System.Collections.Generic;
using System;

public class DiceRoller : MonoBehaviour
{
    public Dice dicePrefab;

    [SerializeField] private Transform diceParent;
    [SerializeField] private List<Dice> diceList;

    private Vector3 tmpDiceInterval = new Vector3(1, 0, 0);
    
    public Dice InstantiateNewDice(DiceType diceType)
    {
        Dice newDice = Instantiate(dicePrefab, transform.position + tmpDiceInterval * diceList.Count, Quaternion.identity);
        newDice.transform.SetParent(diceParent);
        newDice.SetDiceType(diceType);
        newDice.name = "Dice_" + diceList.Count;  // 이름을 유니크하게 지정
        diceList.Add(newDice);
        return newDice;
    }

    public void RemoveAllDice()
    {
        foreach(Dice dice in diceList)
        {
            Destroy(dice.gameObject);
        }
        diceList.Clear();
    }

    public void RollAllDiceNew()
    {
        diceList.Clear();

        List<DiceType> diceTypeList = Inventory.Instance.GetDiceList();

        foreach (DiceType type in diceTypeList)
        {
            InstantiateNewDice(type);
        }

        ReRollAllDice();
    }

    

    public void ReRollAllDice()
    {
        foreach (Dice dice in diceList)
        {
            dice.Roll();
        }
    }
}
