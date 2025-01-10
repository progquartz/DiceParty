using UnityEngine;
using System.Collections.Generic;
using System;

public class DiceRoller : MonoBehaviour
{
    public Dice dicePrefab;

    // 현재 씬에서 관리 중인 모든 주사위 리스트
    [SerializeField] private List<Dice> diceList = new List<Dice>();

    [SerializeField] private Transform diceParent;

    private Vector3 tmpDiceInterval = new Vector3(1, 0, 0);
    
    /// <summary>
    /// 새로운 주사위를 추가한다.
    /// </summary>
    public void AddDice(DiceType diceType)
    {
        // 현재 오브젝트 위치에 주사위를 생성한다고 가정
        Dice newDice = Instantiate(dicePrefab, transform.position + tmpDiceInterval * diceList.Count, Quaternion.identity);
        newDice.transform.SetParent(diceParent); 
        newDice.SetDiceType(diceType);
        newDice.name = "Dice_" + diceList.Count;  // 이름을 유니크하게 지정
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
