using UnityEngine;
using UnityEngine.UI;

public class SkillUISlot : MonoBehaviour
{
    [SerializeField] private Image skillSlotUIImage;
    [SerializeField] private BaseCharacter skillSlotCharacter;

    // ���� ���Կ� ���� Dice (���ٸ� null)
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
            // ƨ�ܳ��� �Ǵ� ���� ���� �ڸ��� �ǵ������� �ʿ�.
            Logger.Log($"�̹� {gameObject.name} ���� �ڸ��� ���ǰ� �ֽ��ϴ�.");
            return false;
        }

        if (skillSlotCharacter.CharacterType != skillDataUI.CharacterType)
        {
            Logger.Log($"{gameObject.name} ���� ��� ĳ���Ͱ� ���� ��ų�� �������� �ʽ��ϴ�.");
            return false;
        }

        storedSkillUI = skillDataUI;

        // �ֻ����� ������ ��ġ�� �̵�.
        skillDataUI.transform.position = transform.position;

        SetSlotColor(activeColors[(int)skillSlotCharacter.CharacterType]);
        return true;
    }

    public void OnSkillUIDetach(SkillUI skill)
    {
        if (storedSkillUI == skill)
        {
            Logger.Log($"{skill.name} ��ų�� ���Կ��� ��������.");
            SetSlotColor(deactivatedColors[(int)skillSlotCharacter.CharacterType]);
            storedSkillUI = null;
        }
        else
        {
            // Ȥ�� ����
            Debug.Log($"[SkillUISlot] �� ���Կ��� {skill.name}��ų�� ���ų� �ٸ� ��ų�� �̹� �����Ǿ��ֽ��ϴ�.");
        }
    }

    private void SetSlotColor(Color slotColor)
    {
        skillSlotUIImage.color = slotColor;
    }

    /// <summary>
    /// �� ������ ���� ��ų�� ������ �ִ���.
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
