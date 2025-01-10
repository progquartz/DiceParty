using UnityEngine;
using System.Collections.Generic;
using System;

public class DiceRoller : MonoBehaviour
{
    public Dice dicePrefab;

    // ���� ������ ���� ���� ��� �ֻ��� ����Ʈ
    [SerializeField] private List<Dice> diceList = new List<Dice>();

    [SerializeField] private Transform diceParent;

    private Vector3 tmpDiceInterval = new Vector3(1, 0, 0);
    
    /// <summary>
    /// ���ο� �ֻ����� �߰��Ѵ�.
    /// </summary>
    public void AddDice(DiceType diceType)
    {
        // ���� ������Ʈ ��ġ�� �ֻ����� �����Ѵٰ� ����
        Dice newDice = Instantiate(dicePrefab, transform.position + tmpDiceInterval * diceList.Count, Quaternion.identity);
        newDice.transform.SetParent(diceParent); 
        newDice.SetDiceType(diceType);
        newDice.name = "Dice_" + diceList.Count;  // �̸��� ����ũ�ϰ� ����
        diceList.Add(newDice);
    }

    public void AddD6()
    {
        AddDice(DiceType.D6);
    }

    public void RollAllDice()
    {
        foreach (Dice dice in diceList)
        {
            dice.Roll();
        }
    }
}
