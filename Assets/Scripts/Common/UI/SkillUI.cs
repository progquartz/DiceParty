using UnityEngine;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private SkillDataSO skillDataSO;
    [SerializeField] private bool[] diceSlotValidity;

    [Header("UI 표시용 텍스트")]
    [SerializeField] private TMP_Text diceNeedText;
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;

    // 스킬 실행 클래스
    private SkillExecutor skillExecutor;

    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        // 필요하다면 싱글톤이나 DI 등을 이용해도 됨
        skillExecutor = new SkillExecutor();
        RefreshDiceSlotValidity();
    }

    private void RefreshDiceSlotValidity()
    {
        if (skillDataSO != null)
        {
            // Dice 조건 맞는지 여부를 초기화.
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

        // 모든 주사위 슬롯이 조건에 맞아 스킬을 사용.
        if (isAllDiceValid)
        {
            Logger.Log($"{diceSlotValidity.Length} 개의 슬롯을 가진 {skillDataSO.skillName} 스킬이 조건에 맞아 실행됩니다.");
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
        // 필요 시 UI도 갱신
        UpdateSkillData();
    }

    public void UseSkill()
    {
        skillExecutor.UseSkill(skillDataSO);
    }

}
