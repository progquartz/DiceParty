using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollUI : MonoBehaviour
{
    [SerializeField] private TMP_Text diceValueText;

    // Dice의 값이 변경될 때 메서드가 호출됨
    public void UpdateUI(int value)
    {
        if (diceValueText != null)
        {
            diceValueText.text = value.ToString();
        }
    }

    public void UpdateUIDummy(int diceMaxValue)
    {
        if(diceValueText != null)
        {
            diceValueText.text = $"D{diceMaxValue}";
        }
    }
}
