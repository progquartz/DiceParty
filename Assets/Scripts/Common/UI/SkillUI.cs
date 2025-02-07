using UnityEngine;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class SkillUI : MonoBehaviour
{
    
    [Header("��ų ������")]
    [SerializeField] private SkillDataSO skillDataSO;

    [Header("�ֻ��� ���� ���� ����")]
    [SerializeField] private bool[] diceSlotValidity;

    [Header("��ϵ� ��ų ����")]
    [SerializeField] private SkillUISlot skillUISlot;

    [Header("UI ǥ�ÿ� �ؽ�Ʈ")]
    
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;
    [SerializeField] private Image skillBackgroundImage;

    [Header("�ֻ��� ���� �ؽ�Ʈ")]
    [SerializeField] private List<TMP_Text> diceNeedTextOneSlot;
    [SerializeField] private List<TMP_Text> diceNeedTextTwoSlot;

    [Header("�ֻ��� ���� 1��¥��")]
    [SerializeField] private GameObject oneDiceSlot;
    [Header("�ֻ��� ���� 2��¥��")]
    [SerializeField] private GameObject twoDiceSlot;


    // ��ų ���� Ŭ����
    private SkillExecutor skillExecutor;

    public event Action<int> OnSkillUse;
    public int SkillUseLeftCount;

    

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
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        BattleManager.Instance.OnBattleStart += OnBattleStart;
        BattleManager.Instance.OnPlayerTurnEnd += OnPlayerTurnEnd;
        BattleManager.Instance.OnPlayerTurnStart += OnPlayerTurnStart;
    }
    private void ReleaseEvents()
    {
        BattleManager.Instance.OnBattleStart -= OnBattleStart;
        BattleManager.Instance.OnPlayerTurnEnd -= OnPlayerTurnEnd;
        BattleManager.Instance.OnPlayerTurnStart -= OnPlayerTurnStart;
    }

    private void RefreshDiceSlotValidity()
    {
        if (skillDataSO != null)
        {
            // Dice ���� �´��� ���θ� �ʱ�ȭ.
            diceSlotValidity = new bool[skillDataSO.diceRequirements.Count];
        }
    }

    public void OnBattleStart()
    {
        DestroyIfNotAttached();
    }
    public void OnPlayerTurnStart()
    {
        // count �ʱ�ȭ
        SkillUseLeftCount = skillDataSO.skillUseCount;
        UpdateVisual();
    }

    public void OnPlayerTurnEnd()
    {
        RefreshDiceSlotValidity();
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
    public void UseSkill(BaseTarget caller)
    {
        if(!IsAttachedToSkillUISlot())
        {
            Logger.LogError("[SkillUI]���Կ� ��ϵ��� ���� ��ų�� ���ǰ� �ֽ��ϴ�!");
            return;
        }

        // �ֻ��� ���� �ʱ�ȭ.
        RefreshDiceSlotValidity();

        // ��� ���� ����.
        SkillUseLeftCount--;

        skillExecutor.UseSkill(skillDataSO, caller);
        OnSkillUse?.Invoke(SkillUseLeftCount);
        UpdateVisual();
        
    }

    // ��ų�� ��ų ���Կ� �־������� ó�� �κ�

    public void OnSkillSlotAttach(SkillUISlot attachedSlot)
    {
        skillUISlot = attachedSlot;
    }

    public void OnSkillSlotDetach()
    {
        skillUISlot = null;
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
        }
        return isDiceNumValid;
    }



    public void OnDiceDetach(Dice dice, int slotSiblingIndex)
    {
        diceSlotValidity[slotSiblingIndex] = false;
    }


    public void CheckDiceValidity()
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
            UseSkill(skillUISlot.GetCharacter());
        }
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
        SkillUseLeftCount = newSkillData.skillUseCount;
        // �ʿ� �� UI�� ����
        UpdateSkillData();
    }

    public bool IsAttachedToSkillUISlot()
    {
        if(skillUISlot != null)
        {
            return true;
        }
        return false;
    }

    private void DestroyIfNotAttached()
    {
        if(!IsAttachedToSkillUISlot())
        {
            DestorySelf();
        }
    }

    public void DestorySelf()
    {
        ReleaseEvents();
        if(skillUISlot != null )
        {
            skillUISlot.OnSkillUIDetach(this);
        }
        // ���Ŀ� �ִϸ��̼� �ʿ�..?
        Destroy(this.gameObject);
        
    }



    private void UpdateVisual()
    {
        if(SkillUseLeftCount < 1)
        {
            skillBackgroundImage.color = Color.gray;
        }
        else
        {
            skillBackgroundImage.color = Color.white;
        }
    }
}
