using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectIconUI : MonoBehaviour
{
    [SerializeField] private TMP_Text effectIconName;
    [SerializeField] private TMP_Text effectIconLore;
    [SerializeField] private TMP_Text effectStackText;

    [SerializeField] private Image effectIcon;

    public void UpdateVisual(EffectKey effectKey, int strength1, int strength2)
    {
        effectIcon.sprite = DataManager.Instance.BuffUIDatas.GetEffectSprite(effectKey);
        effectIconName.text = DataManager.Instance.BuffUIDatas.GetEffectName(effectKey);
        effectIconLore.text = DataManager.Instance.BuffUIDatas.GetEffectDescription(effectKey, strength1, strength2);
        effectStackText.text = strength1.ToString();
    }
}
