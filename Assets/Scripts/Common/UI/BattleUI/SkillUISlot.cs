using UnityEngine;
using UnityEngine.UI;

public class SkillUISlot : MonoBehaviour
{
    [SerializeField] private Image skillSlotUIImage;
    [SerializeField] private BaseCharacter skillSlotCharacter;

    // 현재 슬롯에 들어온 Dice (없다면 null)
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
            // 튕겨내기 또는 원래 슬롯 자리로 되돌려놓기 필요.
            Logger.Log($"이미 {gameObject.name} 슬롯 자리는 사용되고 있습니다.");
            return false;
        }

        if (skillSlotCharacter.CharacterType != skillDataUI.CharacterType)
        {
            Logger.Log($"{gameObject.name} 슬롯 담당 캐릭터가 들어온 스킬에 적합하지 않습니다.");
            return false;
        }

        storedSkillUI = skillDataUI;

        // 주사위를 슬롯의 위치로 이동.
        skillDataUI.transform.position = transform.position;

        SetSlotColor(activeColors[(int)skillSlotCharacter.CharacterType]);
        return true;
    }

    public void OnSkillUIDetach(SkillUI skill)
    {
        if (storedSkillUI == skill)
        {
            Logger.Log($"{skill.name} 스킬이 슬롯에서 빠져나감.");
            SetSlotColor(deactivatedColors[(int)skillSlotCharacter.CharacterType]);
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
