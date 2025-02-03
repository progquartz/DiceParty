using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionDataSO", menuName = "Scriptable Objects/PotionDataSO")]
public class PotionDataSO : ScriptableObject
{
    [Header("포션 이름")]
    public string name;
    [Header("포션 설명")]
    public string lore;
    [Header("포션 스프라이트")]
    public Sprite sprite;
    [Header("효과 배열")]
    public List<SkillEffectData> effectData;
}
