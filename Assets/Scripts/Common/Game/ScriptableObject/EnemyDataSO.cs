using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("적 이름")]
    public string enemyName;
    [Header("적 설명")]
    [TextArea] public string enemyLore;

    [Header("적 스프라이트(애니메이션 사용 스프라이트로 변경 필요)")]
    public Sprite enemytempSprite;

    [Header("적 스탯")]
    public BaseStat stat;

    [Header("적 패턴")]
    public EnemyAttackPatternSO enemyAttackPattern;
}
