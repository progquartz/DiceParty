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
        Dice newDice = Instantiate(dicePrefab);
        newDice.transform.SetParent(diceParent);
        newDice.transform.localPosition = transform.position + tmpDiceInterval * diceList.Count;
        newDice.SetDiceType(diceType);
        newDice.name = "Dice_" + diceList.Count;  // 이름을 유니크하게 설정
        diceList.Add(newDice);
        return newDice;
    }

    

    public void RemoveAllDice()
    {
        if(diceList  != null)
        {
            if(diceList.Count > 0)
            {
                for(int i = diceList.Count - 1; i >= 0; i--)
                {
                    diceList[i].DestroySelf();
                }
            }
        }
    }

    public void SetAllDiceTextDummy()
    {
        if(diceList != null)
        {
            foreach(Dice dice in diceList)
            {
                dice.ShowDiceDummyText();
            }
        }
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
        SetDiceInteractable(true);
    }

    public void RollAllDiceDummy()
    {
        diceList.Clear();

        List<DiceType> diceTypeList = Inventory.Instance.GetDiceList();

        foreach (DiceType type in diceTypeList)
        {
            InstantiateNewDice(type);
        }

        SetAllDiceTextDummy();
        SetDiceInteractable(false);
    }
    

    public void ReRollAllDice()
    {
        foreach (Dice dice in diceList)
        {
            dice.Roll();
        }
    }

    public void SetDiceInteractable(bool state)
    {
        foreach(Dice dice in diceList)
        {
            dice.IsInteractable = state;
        }
    }

    public void RemoveDiceInList(Dice dice)
    {
        diceList.Remove(dice); 
    }
}
