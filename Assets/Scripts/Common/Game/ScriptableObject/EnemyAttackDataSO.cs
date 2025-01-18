using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackDataSO", menuName = "Scriptable Objects/EnemyAttackDataSO")]
public class EnemyAttackDataSO : ScriptableObject
{
    [Header("���ݸ�")]
    public string attackName;
    
    // �������� �𸣰ڴٸ���...
    [Header("���� �� �ϰ� �Ǵ� ���")]
    [TextArea] public string attackText;

    [Header("��ų�� ������ �Ǵ� ����Ʈ")]
    public List<SkillEffectData> damageEffects;

}
