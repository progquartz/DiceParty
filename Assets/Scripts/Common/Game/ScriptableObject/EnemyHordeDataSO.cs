using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// BatteType = 10 * StageNum + BattleType
/// BattleType
/// 0 - 일반 전투
/// 1 - 엘리트 전투
/// 2 - 보스 전투
/// </summary>
public enum BattleType
{
    None = 0,
    Stage1Normal = 10,
    Stage1Elite = 11,
    Stage1Boss = 12,

    Stage2Normal = 20,
    Stage2Elite = 21,
    Stage2Boss = 22,

    Stage3Normal = 30,
    Stage3Elite = 31,
    Stage3Boss = 32,

    Stage4Normal = 40,
    Stage4Elite = 41,
    Stage4Boss = 42,

    Stage5Normal = 50,
    Stage5Elite = 51,
    Stage5Boss = 52,
}

[CreateAssetMenu(fileName = "EnemyHordeDataSO", menuName = "Scriptable Objects/EnemyHordeDataSO")]
public class EnemyHordeDataSO : ScriptableObject
{
    [Header("이 호드가 등장할 스테이지 타입")]
    public BattleType stageType;

    [Header("이 호드가 등치될 때 함께 등장할 적 목록")] 
    public List<EnemyDataSO> enemyList;

    // 등장 가중치 (필요할까?)
    public float spawnWeight;
}
