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
            // 스킬 이름 / 설명 데이터 초기화
            skillNameText.text = SkillDataSO.SkillName;
            skillLoreText.text = SkillDataSO.SkillLore;

            // 다이스 조건 충족 체크.
            diceNeedText.text = SkillDataSO.diceNumLore;
            
        }
    }

    public bool SetSkillData(SkillDataSO skillDataSO)
    {
        if(SkillDataSO != null)
        {
            SkillDataSO = skillDataSO;
            return true; // 정상적으로 바뀜.
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

            // 스킬 타입을 선정.
            string skillType = SkillDataSO.SkillType[i].ToString();
            int skillStrength = SkillDataSO.SkillStrength[i];

            Type skillClassType = Type.GetType(skillType);
            if(skillClassType == null)
            {
                Debug.Log("정의되지 않은 이름의 SKill을 사용하려 합니다.");
                return;
            }

            

            // 타겟팅 옵션을 선정.
            string targetOptionType = SkillDataSO.TargetOption[i].ToString();

            Type targetClassType = Type.GetType(targetOptionType);
            if (targetClassType == null)
            {
                Debug.Log("정의되지 않은 TargetOption을 사용하려 합니다.");
                return;
            }

            // 인스턴스 제작.

            BaseEffect skillEffect = Activator.CreateInstance(skillClassType) as BaseEffect;
            BaseTargetOption targetOption = Activator.CreateInstance(targetClassType) as BaseTargetOption;

            skillEffect.Effect(targetOption.GetTarget(), skillStrength);
        }

    }
}
