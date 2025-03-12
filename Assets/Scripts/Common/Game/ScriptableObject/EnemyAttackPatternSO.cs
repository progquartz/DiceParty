using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackPatternSO", menuName = "Scriptable Objects/EnemyAttackPatternSO")]
public class EnemyAttackPatternSO : ScriptableObject
{
    [Header("적의 공격 이름/설명")]
    public string patternName;
    [TextArea] public string patternDescription;

    [Header("적의 공격하는 노드(루트)")]
    public EnemyAttackNodeSO rootNode;
}
