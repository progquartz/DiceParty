using System;
using TMPro;
using UnityEngine;

public class EnemyLoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyNameText;

    [Obsolete("EnemyDataSO 데이터로 변경 필요.")]
    public void UpdateEnemyLore(string text)
    {
        enemyNameText.text = text;
    }
}
