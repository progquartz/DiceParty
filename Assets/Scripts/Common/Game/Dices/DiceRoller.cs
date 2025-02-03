using UnityEngine;
using System.Collections.Generic;
using System;

public class DiceRoller : MonoBehaviour
{
    public Dice dicePrefab;

    [SerializeField] private Transform diceParent;

    private Vector3 tmpDiceInterval = new Vector3(1, 0, 0);
    
    public Dice InstantiateNewDice(DiceType diceType)
    {
        List<Dice> diceList = Inventory.Instance.GetAllDice();
        Dice newDice = Instantiate(dicePrefab, transform.position + tmpDiceInterval * diceList.Count, Quaternion.identity);
        newDice.transform.SetParent(diceParent);
        newDice.SetDiceType(diceType);
        newDice.name = "Dice_" + diceList.Count;  // 이름을 유니크하게 지정
        return newDice;
    }

    public void RollAllDice()
    {
        foreach (Dice dice in Inventory.Instance.GetAllDice())
        {
            dice.Roll();
        }
    }
}
