using System;
using TMPro;
using UnityEngine;

public class BaseEnemy : BaseTarget
{
    [SerializeField] private EnemyDataSO enemyDataSO;


    [Header("�� UI")]
    [SerializeField] private SpriteRenderer enemySprite;

    [SerializeField] private TMP_Text enemyNameText;
    
    private EnemyPatternExecutor enemyPatternExecutor;

    [Obsolete("�ӽ� test")]
    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        base.Init();
        enemyPatternExecutor = new EnemyPatternExecutor();
        LoadEnemyData();
    }

    private void LoadEnemyData()
    {
        if(enemyDataSO != null) 
        {
            enemySprite.sprite = enemyDataSO.enemytempSprite;
            enemyNameText.text = enemyDataSO.enemyName;
            LoadEnemyStat();
            LoadEnemyPattern();
        }
    }

    private void LoadEnemyStat()
    {
        stat.SetStat(enemyDataSO.stat);
    }

    private void LoadEnemyPattern()
    {
        enemyPatternExecutor.SetPattern(enemyDataSO.enemyAttackPattern);
    }
}
