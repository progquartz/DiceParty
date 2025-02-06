using UnityEngine;
using UnityEngine.UI;

public class SkillUISlot : MonoBehaviour
{
    [SerializeField] private Image skillSlotUIImage;
    [SerializeField] private BaseCharacter skillSlotCharacter;

    // ���� ���Կ� ���� Dice (���ٸ� null)
    private SkillUI storedSkillUI;

    private void Awake()
    {
    }



    public void OnSkillUIAttach(SkillUI skillDataUI)
    {
        if (storedSkillUI != null)
        {
            // ƨ�ܳ��� �Ǵ� ���� ���� �ڸ��� �ǵ������� �ʿ�.
            Logger.Log($"�̹� {gameObject.name} ���� �ڸ��� ���ǰ� �ֽ��ϴ�.");
            SetSlotColor(Color.red);
            return;
        }

        storedSkillUI = skillDataUI;
        
        Debug.Log($"[SkillUISlot] {skillDataUI.name} ��ų�� ���Կ� ����.");


        // �ֻ����� ������ ��ġ�� �̵�.
        skillDataUI.transform.position = transform.position;

        SetSlotColor(Color.green);
    }

    public void OnSkillUIDetach(SkillUI skill)
    {
        if (storedSkillUI == skill)
        {
            Logger.Log($"{skill.name} ��ų�� ���Կ��� ��������.");
            SetSlotColor(Color.grey);
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
