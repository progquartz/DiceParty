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
    }

    public void EmptyPotion()
    {
        this.potionData = null;
    }

    public bool UsePotion()
    {
        if (potionData != null)
        {
            SkillExecutor skillExecutor = _skillExecutor;
            foreach (SkillEffectData skillData in potionData.effectData)
            {
                skillExecutor.UsePotion(skillData);
            }
            potionData = null;
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
