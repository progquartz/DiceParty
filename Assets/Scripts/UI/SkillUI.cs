using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillUI : MonoBehaviour
{

    public SkillDataSO SkillDataSO;

    [SerializeField] private TMP_Text diceNeedText;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;

    public void Init()
    {

    }
 
    // Update is called once per frame
    void Update()
    {
        UpdateSkillData();
    }

    private void UpdateSkillData()
    {
        if(SkillDataSO != null)
        {
            // ��ų �̸� / ���� ������ �ʱ�ȭ
            skillNameText.text = SkillDataSO.SkillName;
            skillLoreText.text = SkillDataSO.SkillLore;

            // ���̽� ���� ���� üũ.
            diceNeedText.text = SkillDataSO.diceNumLore;
            
        }
    }

    public bool SetSkillData(SkillDataSO skillDataSO)
    {
        if(SkillDataSO != null)
        {
            SkillDataSO = skillDataSO;
            return true; // ���������� �ٲ�.
        }
        else
        {
            SkillDataSO = skillDataSO;
            return false;
        }
        
    }

    public void UseSkill()
    {
        for(int i = 0; i < SkillDataSO.SkillType.Count; i++)
        {

            // ��ų Ÿ���� ����.
            string skillType = SkillDataSO.SkillType[i].ToString();
            int skillStrength = SkillDataSO.SkillStrength[i];

            Type skillClassType = Type.GetType(skillType);
            if(skillClassType == null)
            {
                Debug.Log("���ǵ��� ���� �̸��� SKill�� ����Ϸ� �մϴ�.");
                return;
            }

            

            // Ÿ���� �ɼ��� ����.
            string targetOptionType = SkillDataSO.TargetOption[i].ToString();

            Type targetClassType = Type.GetType(targetOptionType);
            if (targetClassType == null)
            {
                Debug.Log("���ǵ��� ���� TargetOption�� ����Ϸ� �մϴ�.");
                return;
            }

            // �ν��Ͻ� ����.

            BaseEffect skillEffect = Activator.CreateInstance(skillClassType) as BaseEffect;
            BaseTargetOption targetOption = Activator.CreateInstance(targetClassType) as BaseTargetOption;

            skillEffect.Effect(targetOption.GetTarget(), skillStrength);
        }

    }
}
