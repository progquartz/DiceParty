using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyAttackDataSO", menuName = "Scriptable Objects/EnemyAttackDataSO")]
public class EnemyAttackDataSO : ScriptableObject
{
    [Header("공격명")]
    public string attackName;
    
    // 넣을지는 모르겠다만은...
    [Header("공격 시 하게 되는 대사")]
    [TextArea] public string attackText;

    [Header("스킬이 가지게 되는 이펙트")]
    public List<SkillEffectData> damageEffects;

}
