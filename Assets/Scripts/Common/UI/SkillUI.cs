using UnityEngine;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;

public class SkillUI : MonoBehaviour
{
    
    [Header("��ų ������")]
    [SerializeField] private SkillDataSO skillDataSO;

    [Header("�ֻ��� ���� ���� ����")]
    [SerializeField] private bool[] diceSlotValidity;
    [Header("��ų ���Կ� ��ϵǾ����� ����")]
    [SerializeField] private bool isAttachedToSkillUISlot;

    [Header("UI ǥ�ÿ� �ؽ�Ʈ")]
    
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;

    [Header("�ֻ��� ���� �ؽ�Ʈ")]
    [SerializeField] private List<TMP_Text> diceNeedTextOneSlot;
    [SerializeField] private List<TMP_Text> diceNeedTextTwoSlot;

    [Header("�ֻ��� ���� 1��¥��")]
    [SerializeField] private GameObject oneDiceSlot;
    [Header("�ֻ��� ���� 2��¥��")]
    [SerializeField] private GameObject twoDiceSlot;

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
        CheckDiceSlotCount();
    }

    private void RefreshDiceSlotValidity()
    {
        if (skillDataSO != null)
        {
            // Dice ���� �´��� ���θ� �ʱ�ȭ.
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
                Logger.LogError($"{skillDataSO.skillName} �̸��� ���� ��ų�� ���������� ������ �ֻ��� ���� ������ �ʿ�� �մϴ�.");
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

    /// <summary>
    /// ��ų ���
    /// </summary>
    public void UseSkill()
    {
        if(!isAttachedToSkillUISlot)
        {
            Logger.LogError("[SkillUI]���Կ� ��ϵ��� ���� ��ų�� ���ǰ� �ֽ��ϴ�!");
            return;
        }
        skillExecutor.UseSkill(skillDataSO);
    }

    // ��ų�� ��ų ���Կ� �־������� ó�� �κ�

    public void OnSkillSlotAttach()
    {
        isAttachedToSkillUISlot = true;
    }

    public void OnSkillSlotDetach()
    {
        isAttachedToSkillUISlot = false;
    }



    // �ֻ��� ���� ���� �ֻ��� ó�� �κ�

    /// <summary>
    /// �ֻ����� ��ų�� ���Կ� ���� �ֻ����� ������ üũ.
    /// </summary>
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


    // ���� ������ ó��

    public void SetSkillData(SkillDataSO newSkillData)
    {
        skillDataSO = newSkillData;
        // �ʿ� �� UI�� ����
        UpdateSkillData();
    }

    public bool IsAttachedToSkillUISlot()
    {
        return isAttachedToSkillUISlot;
    }

    public void DestorySelf()
    {
        // ���Ŀ� �ִϸ��̼� �ʿ�..?
        Destroy(this.gameObject);
    }

}
