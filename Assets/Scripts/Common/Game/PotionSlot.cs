using UnityEngine;

public class PotionSlot : MonoBehaviour
{
    [SerializeField] private PotionDataSO potionData;

    private SkillExecutor _skillExecutor;
    [SerializeField] private PotionSlotUI potionUI;

    private void Awake()
    {
        _skillExecutor = new SkillExecutor();
    }

    public void SetPotion(PotionDataSO potionData)
    {
        this.potionData = potionData;
        potionUI.UpdateSprite(potionData);
        potionUI.UpdateLoreTexts(potionData);
    }

    public void EmptyPotion()
    {
        this.potionData = null;
        potionUI.OnPotionSlotClear();
    }

    public bool UsePotion()
    {
        Debug.Log($"{gameObject.name}오브젝트의 포션 슬롯을 사용합니다.");
        if (this.potionData != null)
        {
            SkillExecutor skillExecutor = _skillExecutor;
            foreach (SkillEffectData skillData in potionData.effectData)
            {
                skillExecutor.UsePotion(skillData);
            }
            potionData = null;
            potionUI.OnPotionSlotClear();
            return true;
        }
        else
        {
            Logger.LogError($"포션이 없는데 호출되었습니다.");
            return false;
        }
    }

    public bool GetPotionAvailability()
    {
        if (potionData != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public PotionDataSO GetPotionData()
    {
        return potionData;
    }
}
