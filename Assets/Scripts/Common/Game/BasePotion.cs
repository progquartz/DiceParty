using UnityEngine;

public class BasePotion : MonoBehaviour
{
    [SerializeField] private PotionDataSO potionData;

    private SpriteRenderer potionSprite;
    private SkillExecutor _skillExecutor;


    public void SetPotion(PotionDataSO potionData)
    {
        this.potionData = potionData;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (potionData != null)
        {
            potionSprite.sprite = potionData.sprite;
        }
        else
        {
            potionSprite.sprite = null;
        }
        
    }

    public bool UsePotion()
    {
        if(potionData != null)
        {
            SkillExecutor skillExecutor = _skillExecutor;
            foreach (SkillEffectData skillData in potionData.effectData)
            {
                skillExecutor.UseSkill(skillData);
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
}
