using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollUI : MonoBehaviour
{
    [SerializeField] private TMP_Text diceValueText;

    // Dice가 굴려질 때마다 이 메서드가 호출
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
