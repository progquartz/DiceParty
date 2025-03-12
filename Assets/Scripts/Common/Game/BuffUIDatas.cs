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
            Logger.LogError($"{effect.ToString()} 효과의 description이 존재하지 않습니다.");
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
        Logger.LogWarning("비정상적인 데이터를 입력받아 string을 생성하지 못했습니다.");
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
