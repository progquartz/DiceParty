using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SkillSlotUI : MonoBehaviour
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
}
