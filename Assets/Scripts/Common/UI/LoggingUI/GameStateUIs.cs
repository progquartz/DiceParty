using TMPro;
using UnityEngine;

public class GameStateUIs : MonoBehaviour
{
    [Header("소지 골드")]
    [SerializeField] private TMP_Text goldText;

    [Header("현재 스테이지")]
    [SerializeField] private TMP_Text stageText;


    private void Update()
    {
        goldText.text = $"{Inventory.Instance.gold.ToString()}G";
        stageText.text = $"Stage{MapManager.Instance.currentStageNum.ToString()}";
    }
}
