using System;
using TMPro;
using UnityEngine;

public class BaseEnemy : BaseTarget
{
    [SerializeField] private EnemyDataSO enemyDataSO;


    [Header("적 UI")]
    [SerializeField] private SpriteRenderer enemySprite;

    [SerializeField] private TMP_Text enemyNameText;
    
    private EnemyPatternExecutor enemyPatternExecutor;

    [Obsolete("임시 test")]
    private void Awake()
    {
        Init();
    }

    public void Init(EnemyDataSO dataSO)
    {
        base.Init();
        RegisterEvents();
        enemyPatternExecutor = new EnemyPatternExecutor();
        ChangeDataSO(dataSO);
        LoadEnemyData();
    }

    /// <summary>
    /// 적 턴 시작 
    /// </summary>
    private void OnEnemyTurnStart()
    {
        // 턴 시작 / 턴 별 효과 계산.
        base.EffectCalcOnTurnStart();
    }

    /// <summary>
    /// 적 턴 끝
    /// </summary>
    private void OnEnemyTurnEnd()
    {
        base.EffectCalcOnTurnEnd();
    }



    private void RegisterEvents()
    {
        BattleManager.Instance.OnEnemyTurnStart += OnEnemyTurnStart;
        BattleManager.Instance.OnEnemyTurnEnd += OnEnemyTurnEnd;
    }

    private void ReleaseEvents()
    {
        BattleManager.Instance.OnEnemyTurnStart -= OnEnemyTurnStart;
        BattleManager.Instance.OnEnemyTurnEnd -= OnEnemyTurnEnd;
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
        enemyPatternExecutor.ExecuteNextAttack(this);
    }

    private void OnDestroy()
    {
        ReleaseEvents();
    }
}
