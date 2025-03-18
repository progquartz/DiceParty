using TMPro;
using UnityEngine;

public class GameStateUIs : MonoBehaviour
{
    [Header("���� ���")]
    [SerializeField] private TMP_Text goldText;

    [Header("���� ��������")]
    [SerializeField] private TMP_Text stageText;


    private void Update()
    {
        goldText.text = $"{Inventory.Instance.gold.ToString()}G";
        stageText.text = $"Stage{MapManager.Instance.currentStageNum.ToString()}";
    }
}
