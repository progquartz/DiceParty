using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackPatternSO", menuName = "Scriptable Objects/EnemyAttackPatternSO")]
public class EnemyAttackPatternSO : ScriptableObject
{
    [Header("공격 패턴 이름/설명")]
    public string patternName;
    [TextArea] public string patternDescription;

    [Header("공격 패턴의 시작 노드(루트)")]
    public EnemyAttackNodeSO rootNode;
}
