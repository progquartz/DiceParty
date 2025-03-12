using UnityEngine;
using UnityEngine.UI;

public class SkillUISlot : MonoBehaviour
{
    [SerializeField] private Image skillSlotUIImage;
    [SerializeField] private BaseCharacter skillSlotCharacter;

    // 슬롯에 저장된 스킬 (최대 하나, null일 수도 있음)
    private SkillUI storedSkillUI;

    [SerializeField] private Color[] activeColors;
    [SerializeField] private Color[] deactivatedColors;


    private void Awake()
    {
        RegisterEvents();
        SetSlotColor(deactivatedColors[(int)skillSlotCharacter.CharacterType]);
    }

    private void RegisterEvents()
    {
        if(skillSlotCharacter != null)
        {
            skillSlotCharacter.OnDead += OnCharacterDead;
            skillSlotCharacter.OnRevive += OnCharacterRevive;
        }
    }

    private void OnCharacterDead(BaseTarget character)
    {
        if(storedSkillUI != null)
        {
            storedSkillUI.OnOwnerDead();
        }
    }

    private void OnCharacterRevive(BaseTarget character)
    {

    }

    public bool AttachNewSkillUI(SkillDataSO skillDataSO)
    {
        SkillUI skillDataUI = LootingManager.Instance.SkillUiSpawner.SpawnSkillUI(skillDataSO);
        bool isAttached = OnSkillUIAttach(skillDataUI);
        if(isAttached)
        {
            skillDataUI.OnSkillSlotAttach(this);
        }
        return isAttached;
    }

    public bool OnSkillUIAttach(SkillUI skillDataUI)
    {
        if (storedSkillUI != null)
        {
            // 슬롯에 이미 스킬이 있는 경우 오류 로그 출력
            Logger.Log($"{gameObject.name} 슬롯에는 이미 스킬이 있습니다.");
            return false;
        }

        if (skillSlotCharacter.CharacterType != skillDataUI.CharacterType)
        {
            Logger.Log($"{gameObject.name} 슬롯의 캐릭터가 가진 스킬이 아닙니다.");
            return false;
        }

        storedSkillUI = skillDataUI;

        // 주사위가 있으면 위치로 이동.
        skillDataUI.transform.position = transform.position;

        SetSlotColor(activeColors[(int)skillSlotCharacter.CharacterType]);
        return true;
    }

    public void OnSkillUIDetach(SkillUI skill)
    {
        if (storedSkillUI == skill)
        {
            Logger.Log($"{skill.name} 스킬이 슬롯에서 제거됨.");
            SetSlotColor(deactivatedColors[(int)skillSlotCharacter.CharacterType]);
            storedSkillUI = null;
        }
        else
        {
            // 혹시 모름
            Debug.Log($"[SkillUISlot] 이 슬롯에는 {skill.name}스킬이 없거나 다른 스킬이 이미 저장되어있습니다.");
        }
    }

    private void SetSlotColor(Color slotColor)
    {
        skillSlotUIImage.color = slotColor;
    }

    /// <summary>
    /// 이 슬롯에 저장된 스킬이 있는지.
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
