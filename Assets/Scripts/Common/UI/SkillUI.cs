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
    
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;

    [Header("주사위 조건 텍스트")]
    [SerializeField] private List<TMP_Text> diceNeedTextOneSlot;
    [SerializeField] private List<TMP_Text> diceNeedTextTwoSlot;

    [Header("주사위 슬롯 1개짜리")]
    [SerializeField] private GameObject oneDiceSlot;
    [Header("주사위 슬롯 2개짜리")]
    [SerializeField] private GameObject twoDiceSlot;

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
        CheckDiceSlotCount();
    }

    private void RefreshDiceSlotValidity()
    {
        if (skillDataSO != null)
        {
            // Dice 조건 맞는지 여부를 초기화.
            diceSlotValidity = new bool[skillDataSO.diceRequirements.Count];
        }
    }

    private void CheckDiceSlotCount()
    {
        if(skillDataSO != null)
        {
            if( skillDataSO.diceRequirements.Count == 1)
            {
                oneDiceSlot.SetActive(true);
                twoDiceSlot.SetActive(false);
            }
            else if(skillDataSO.diceRequirements.Count == 2)
            {
                oneDiceSlot.SetActive(false);
                twoDiceSlot.SetActive(true);
            }
            else
            {
                Logger.LogError($"{skillDataSO.skillName} 이름을 가진 스킬이 비정상적인 개수의 주사위 슬롯 개수를 필요로 합니다.");
            }
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

            if(skillDataSO.diceRequirements.Count == 1)
            {
                diceNeedTextOneSlot[0].text = skillDataSO.diceRequirements[0].diceNumLore;
            }
            else if( skillDataSO.diceRequirements.Count == 2)
            {
                diceNeedTextTwoSlot[0].text = skillDataSO.diceRequirements[0].diceNumLore;
                diceNeedTextTwoSlot[1].text = skillDataSO.diceRequirements[1].diceNumLore;
            }
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
