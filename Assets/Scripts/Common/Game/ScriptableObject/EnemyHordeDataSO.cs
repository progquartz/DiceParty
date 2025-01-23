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
    [Header("�� ȣ�尡 ������ �������� Ÿ��")]
    public StageType stageType;

    [Header("�� ȣ�尡 ��ġ�� �� �Բ� ������ �� ���")] 
    public List<EnemyDataSO> enemyList;

    // ���� ���� (�ʿ��ұ�?)
    public float spawnWeight;
}
