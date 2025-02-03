using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionDataSO", menuName = "Scriptable Objects/PotionDataSO")]
public class PotionDataSO : ScriptableObject
{
    [Header("���� �̸�")]
    public string name;
    [Header("���� ����")]
    public string lore;
    [Header("���� ��������Ʈ")]
    public Sprite sprite;
    [Header("ȿ�� �迭")]
    public List<SkillEffectData> effectData;
}
