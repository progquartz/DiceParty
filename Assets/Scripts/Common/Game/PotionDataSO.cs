using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PotionDataSO : MonoBehaviour
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
