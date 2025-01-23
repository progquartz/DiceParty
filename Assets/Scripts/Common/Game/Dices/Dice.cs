using System.Xml.Linq;
using UnityEngine;

public enum DiceType
{
    D4 = 4,
    D6 = 6,
    D12 = 12,
    D16 = 16,
    D20 = 20
}
public class Dice : MonoBehaviour
{
    public int CurNum;
    public DiceType Type;
    private DiceRollUI diceRollUI;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        diceRollUI = GetComponent<DiceRollUI>();
    }

    public void SetDiceType(DiceType type)
    {
        Type = type;
    }

    public int Roll()
    {
        CurNum = Random.Range(1, (int)Type + 1);
        diceRollUI.UpdateUI(CurNum);
        return CurNum;
    }
}
