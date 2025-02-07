using UnityEngine;
using UnityEngine.UI;

public class SkillUISlot : MonoBehaviour
{
    [SerializeField] private Image skillSlotUIImage;
    [SerializeField] private BaseCharacter skillSlotCharacter;

    // 현재 슬롯에 들어온 Dice (없다면 null)
    private SkillUI storedSkillUI;

    private void Awake()
    {
    }



    public void OnSkillUIAttach(SkillUI skillDataUI)
    {
        if (storedSkillUI != null)
        {
            // 튕겨내기 또는 원래 슬롯 자리로 되돌려놓기 필요.
            Logger.Log($"이미 {gameObject.name} 슬롯 자리는 사용되고 있습니다.");
            SetSlotColor(Color.red);
            return;
        }

        storedSkillUI = skillDataUI;
        
        Debug.Log($"[SkillUISlot] {skillDataUI.name} 스킬을 슬롯에 보관.");


        // 주사위를 슬롯의 위치로 이동.
        skillDataUI.transform.position = transform.position;

        SetSlotColor(Color.green);
    }

    public void OnSkillUIDetach(SkillUI skill)
    {
        if (storedSkillUI == skill)
        {
            Logger.Log($"{skill.name} 스킬이 슬롯에서 빠져나감.");
            SetSlotColor(Color.grey);
            storedSkillUI = null;
        }
        else
        {
            // 혹은 무시
            Debug.Log($"[SkillUISlot] 이 슬롯에는 {skill.name}스킬이 없거나 다른 스킬이 이미 지정되어있습니다.");
        }
    }

    private void SetSlotColor(Color slotColor)
    {
        skillSlotUIImage.color = slotColor;
    }

    /// <summary>
    /// 이 슬롯이 현재 스킬을 가지고 있는지.
    /// </summary>
    public bool HasSkill()
    {
        return storedSkillUI != null;
    }

    public SkillUI GetSkill()
    {
        return storedSkillUI;
    }

    public BaseCharacter GetCharacter() { return skillSlotCharacter; }
}
