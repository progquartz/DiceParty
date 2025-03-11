using System.Collections.Generic;
using UnityEngine;

public class BuffUIDatas : MonoBehaviour
{

    public List<EffectData> effectDataList;
    
    private Dictionary<EffectClassName, EffectData> effectDataDict = new Dictionary<EffectClassName, EffectData>();

    private void Awake()
    {
        foreach (EffectData data in effectDataList)
        {
            effectDataDict[data.effectClass] = data;
        }
    }

    public Sprite GetEffectSprite(EffectClassName effect)
    {
        return effectDataDict.ContainsKey(effect) ? effectDataDict[effect].effectSprite : null;
    }

    public string GetEffectDescription(EffectClassName effect, int strength1, int strength2)
    {
        string[] description = effectDataDict.ContainsKey(effect) ? effectDataDict[effect].description : null;
        switch(description.Length)
        {
            case 1:
                return description[0];
            case 2:
                return description[0] + strength1.ToString() + description[1];
            case 3:
                return description[0] + strength1.ToString() + description[1] + strength2.ToString() + description[2];
        }
        Logger.LogWarning("비정상적인 데이터를 입력받아 string이 만들어지지 못했습니다.");
        return null;
    }

    public string GetEffectName(EffectClassName effect)
    {
        return effectDataDict.ContainsKey(effect) ? effectDataDict[effect].name : "Buff_Ui_Name";
    }
}

[System.Serializable]
public class EffectData
{
    public EffectClassName effectClass;
    public Sprite effectSprite;
    public string name;
    public string[] description;
}
