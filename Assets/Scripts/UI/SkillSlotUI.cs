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
}
