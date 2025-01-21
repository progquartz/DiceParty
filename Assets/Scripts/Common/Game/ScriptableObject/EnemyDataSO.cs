using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("몹 이름")]
    public string enemyName;
    [Header("몹 설명")]
    [TextArea] public string enemyLore;

    [Header("적 스프라이트(애니메이션 기반 스프라이트로 변경 필요)")]
    public Sprite enemytempSprite;

    [Header("몹 스탯")]
    public BaseStat stat;

    [Header("몹 패턴")]
    public EnemyAttackPatternSO enemyAttackPattern;
}
