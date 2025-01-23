using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum StageType
{
    Stage1Normal,
    Stage1Elite,
    Stage1Boss,

    Stage2Normal,
    Stage2Elite,
    Stage2Boss,
}

[CreateAssetMenu(fileName = "EnemyHordeDataSO", menuName = "Scriptable Objects/EnemyHordeDataSO")]
public class EnemyHordeDataSO : ScriptableObject
{
    [Header("이 호드가 등장할 스테이지 타입")]
    public StageType stageType;

    [Header("이 호드가 배치될 때 함께 등장할 적 목록")] 
    public List<EnemyDataSO> enemyList;

    // 스폰 비중 (필요할까?)
    public float spawnWeight;
}
