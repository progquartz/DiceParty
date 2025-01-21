using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Scriptable Objects/EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    [Header("�� �̸�")]
    public string enemyName;
    [Header("�� ����")]
    [TextArea] public string enemyLore;

    [Header("�� ��������Ʈ(�ִϸ��̼� ��� ��������Ʈ�� ���� �ʿ�)")]
    public Sprite enemytempSprite;

    [Header("�� ����")]
    public BaseStat stat;

    [Header("�� ����")]
    public EnemyAttackPatternSO enemyAttackPattern;
}
