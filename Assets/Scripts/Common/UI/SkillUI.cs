using UnityEngine;
using TMPro;
using System;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillDataSO skillDataSO;

    [Header("UI ǥ�ÿ� �ؽ�Ʈ")]
    [SerializeField] private TMP_Text diceNeedText;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;

    // ��ų ���� Ŭ����
    private SkillExecutor skillExecutor;

    private void Awake()
    {
        // �ʿ��ϴٸ� �̱����̳� DI ���� �̿��ص� ��
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
        // �ʿ� �� UI�� ����
        UpdateSkillData();
    }

    public void UseSkill()
    {
        skillExecutor.UseSkill(skillDataSO);
    }

    [Obsolete("���� ��������� ���� ���")]
    public bool CheckDiceAvailability(Dice newDice)
    {
        return false;
    }
}
