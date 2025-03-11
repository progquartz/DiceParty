using System.Collections.Generic;
using UnityEngine;

public class BuffUIDatas : MonoBehaviour
{

    public List<EffectData> effectDataList;
    
    private Dictionary<EffectKey, EffectData> effectDataDict = new Dictionary<EffectKey, EffectData>();

    private void Awake()
    {
        foreach (EffectData data in effectDataList)
        {
            effectDataDict[data.effectClass] = data;
        }
    }

    public Sprite GetEffectSprite(EffectKey effect)
    {
        return effectDataDict.ContainsKey(effect) ? effectDataDict[effect].effectSprite : null;
    }

    public string GetEffectDescription(EffectKey effect, int strength1, int strength2)
    {
        string[] description = effectDataDict.ContainsKey(effect) ? effectDataDict[effect].description : null;
        if (description == null)
        {
            Logger.LogError($"{effect.ToString()} ȿ���� description�� �������� �ʽ��ϴ�.");
        }
        switch(description.Length)
        {
            case 1:
                return description[0];
            case 2:
                return description[0] + strength1.ToString() + description[1];
            case 3:
                return description[0] + strength1.ToString() + description[1] + strength2.ToString() + description[2];
        }
        Logger.LogWarning("���������� �����͸� �Է¹޾� string�� ��������� ���߽��ϴ�.");
        return null;
    }

    public string GetEffectName(EffectKey effect)
    {
        return effectDataDict.ContainsKey(effect) ? effectDataDict[effect].name : "Buff_Ui_Name";
    }
}

[System.Serializable]
public class EffectData
{
    public EffectKey effectClass;
    public Sprite effectSprite;
    public string name;
    public string[] description;
}
