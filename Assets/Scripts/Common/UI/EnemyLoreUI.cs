using System;
using TMPro;
using UnityEngine;

public class EnemyLoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text enemyNameText;

    [Obsolete("EnemyDataSO ������� ���� ���� �ʿ�.")]
    public void UpdateEnemyLore(string text)
    {
        enemyNameText.text = text;
    }
}
