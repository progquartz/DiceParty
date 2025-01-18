using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackPatternSO", menuName = "Scriptable Objects/EnemyAttackPatternSO")]
public class EnemyAttackPatternSO : ScriptableObject
{
    [Header("���� ���� �̸�/����")]
    public string patternName;
    [TextArea] public string patternDescription;

    [Header("���� ������ ���� ���(��Ʈ)")]
    public EnemyAttackNodeSO rootNode;
}
