using UnityEngine;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillDataSO skillDataSO;
    [SerializeField] private bool[] diceSlotValidity;

    [Header("UI ǥ�ÿ� �ؽ�Ʈ")]
    [SerializeField] private TMP_Text diceNeedText;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;

    // ��ų ���� Ŭ����
    private SkillExecutor skillExecutor;

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        // �ʿ��ϴٸ� �̱����̳� DI ���� �̿��ص� ��
        skillExecutor = new SkillExecutor();
        RefreshDiceSlotValidity();
    }

    private void RefreshDiceSlotValidity()
    {
        if (skillDataSO != null)
        {
            // Dice ���� �´��� ���θ� �ʱ�ȭ.
            diceSlotValidity = new bool[skillDataSO.diceRequirements.Count];
        }
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

            //diceNeedText.text = skillDataSO.diceRequirements.;
        }
    }



    public bool OnDiceAttach(Dice dice, int slotSiblingIndex)
    {
        bool isDiceNumValid = DiceCheck(dice, skillDataSO.diceRequirements[slotSiblingIndex]);
        if (isDiceNumValid)
        {
            diceSlotValidity[slotSiblingIndex] = true;
            CheckDiceValidity();
        }
        return isDiceNumValid;
    }

    private void CheckDiceValidity()
    {
        bool isAllDiceValid = true;
        foreach (bool data in diceSlotValidity)
        {
            if (data == false)
            {
                isAllDiceValid = false;
            }
        }

        // ��� �ֻ��� ������ ���ǿ� �¾� ��ų�� ���.
        if (isAllDiceValid)
        {
            Logger.Log($"{diceSlotValidity.Length} ���� ������ ���� {skillDataSO.skillName} ��ų�� ���ǿ� �¾� ����˴ϴ�.");
            UseSkill();
        }
    }

    public void OnDiceDetach(Dice dice, int slotSiblingIndex)
    {
        diceSlotValidity[slotSiblingIndex] = false;
    }

    private bool DiceCheck(Dice dice, DiceRequirementData diceData)
    {
        if(diceData.diceNum[dice.CurNum] == '1')
        {
            return true;
        }
        else
        {
            return false;
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

}
