using UnityEngine;
using TMPro;
using System;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillDataSO skillDataSO;

    [Header("UI 표시용 텍스트")]
    [SerializeField] private TMP_Text diceNeedText;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;

    // 스킬 실행 클래스
    private SkillExecutor skillExecutor;

    private void Awake()
    {
        // 필요하다면 싱글톤이나 DI 등을 이용해도 됨
        skillExecutor = new SkillExecutor();
    }

    private void Update()
    {
        UpdateSkillData();
    }

    private void UpdateSkillData()
    {
        if (skillDataSO != null)
        {
            skillNameText.text = skillDataSO.skillName;
            skillLoreText.text = skillDataSO.skillLore;
            diceNeedText.text = skillDataSO.diceNumLore;
        }
    }

    public void SetSkillData(SkillDataSO newSkillData)
    {
        skillDataSO = newSkillData;
        // 필요 시 UI도 갱신
        UpdateSkillData();
    }

    public void UseSkill()
    {
        skillExecutor.UseSkill(skillDataSO);
    }

    [Obsolete("현재 만들어지지 않은 기능")]
    public bool CheckDiceAvailability(Dice newDice)
    {
        return false;
    }
}
