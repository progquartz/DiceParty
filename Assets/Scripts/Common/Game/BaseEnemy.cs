using System;
using TMPro;
using UnityEngine;

public class BaseEnemy : BaseTarget
{
    [SerializeField] private EnemyDataSO enemyDataSO;


    [Header("Àû UI")]
    [SerializeField] private SpriteRenderer enemySprite;

    [SerializeField] private TMP_Text enemyNameText;
    
    private EnemyPatternExecutor enemyPatternExecutor;

    [Obsolete("ÀÓ½Ã test")]
    private void Awake()
    {
        Init();
    }

    public void Init(EnemyDataSO dataSO)
    {
        base.Init();
        enemyPatternExecutor = new EnemyPatternExecutor();
        ChangeDataSO(dataSO);
        LoadEnemyData();
    }

    private void ChangeDataSO(EnemyDataSO enemyDataSO)
    {
        this.enemyDataSO = enemyDataSO;
    }

    private void LoadEnemyData()
    {
        if(enemyDataSO != null) 
        {
            gameObject.name = enemyDataSO.name;
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

    public void AttackOnPattern()
    {
        enemyPatternExecutor.ExecuteNextAttack();
    }
}
