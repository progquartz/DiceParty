using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollUI : MonoBehaviour
{
    [SerializeField] private TMP_Text diceValueText;

    // Dice�� ������ ������ �� �޼��尡 ȣ��
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
