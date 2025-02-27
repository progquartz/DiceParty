using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// BatteType = 10 * StageNum + BattleType
/// BattleType
/// 0 - �Ϲ� ����
/// 1 - ����Ʈ ����
/// 2 - ���� ����
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
}

[CreateAssetMenu(fileName = "EnemyHordeDataSO", menuName = "Scriptable Objects/EnemyHordeDataSO")]
public class EnemyHordeDataSO : ScriptableObject
{
    [Header("�� ȣ�尡 ������ �������� Ÿ��")]
    public BattleType stageType;

    [Header("�� ȣ�尡 ��ġ�� �� �Բ� ������ �� ���")] 
    public List<EnemyDataSO> enemyList;

    // ���� ���� (�ʿ��ұ�?)
    public float spawnWeight;
}
