using System;
using TMPro;
using UnityEngine;

public class EnemyLoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyNameText;

    [Obsolete("EnemyDataSO 기반으로 새로 제작 필요.")]
    public void UpdateEnemyLore(string text)
    {
        enemyNameText.text = text;
    }
}
