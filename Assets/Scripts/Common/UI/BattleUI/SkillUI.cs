using UnityEngine;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.CompilerServices;
using System.Linq;

public class SkillUI : MonoBehaviour
{
    
    [Header("��ų ������")]
    [SerializeField] private SkillDataSO skillDataSO;
    public CharacterType CharacterType { get { return skillDataSO.CharacterType; } }

    [Header("�ֻ��� ���� ���� ����")]
    [SerializeField] private bool[] diceSlotValidity;

    [Header("��ϵ� ��ų ����")]
    [SerializeField] public SkillUISlot skillUISlot;
    private SkillUISlot currentSlot = null;

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

    [Header("�ֻ��� ���� UI")]
    [SerializeField] private SkillDiceSlotUI[] diceSlotUI;

    [SerializeField] private Color[] activeColors;
    [SerializeField] private Color[] deactivatedColors;


    // ��ų ���� Ŭ����
    private SkillExecutor skillExecutor;

    public event Action<bool> OnSkillToggle;
    public int SkillUseLeftCount;

    public void Init(SkillDataSO skillData)
    {
        SetSkillData(skillData);
        // �ʿ��ϴٸ� �̱����̳� DI ���� �̿��ص� ��
        skillExecutor = new SkillExecutor();
        RefreshDiceSlotValidity();
        CheckDiceSlotCount();
        RegisterEvents();
        UpdateVisual();
    }

    private void RegisterEvents()
    {
        MapManager.Instance.OnMoveRoom += OnMapMove;
        BattleManager.Instance.OnBattleStart += OnBattleStart;
        BattleManager.Instance.OnPlayerTurnEnd += OnPlayerTurnEnd;
        BattleManager.Instance.OnPlayerTurnStart += OnPlayerTurnStart;
        BattleManager.Instance.OnBattleEnd += OnBattleEnd;
        
    }
    private void ReleaseEvents()
    {
        MapManager.Instance.OnMoveRoom -= OnMapMove;
        BattleManager.Instance.OnBattleStart -= OnBattleStart;
        BattleManager.Instance.OnPlayerTurnEnd -= OnPlayerTurnEnd;
        BattleManager.Instance.OnPlayerTurnStart -= OnPlayerTurnStart;
        BattleManager.Instance.OnBattleEnd -= OnBattleEnd;
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
        UpdateVisual();
    }

    private void OnBattleEnd()
    {
        SkillUseLeftCount = skillDataSO.skillUseCount;
        UpdateVisual();
    }

    private void OnMapMove()
    {
        DestroyIfNotAttached();
    }

    public void OnPlayerTurnStart()
    {
        // count �ʱ�ȭ
        SkillUseLeftCount = skillDataSO.skillUseCount;
        foreach(var diceSlot in diceSlotUI)
        {
            diceSlot.ActivateDiceSlot();
        }
        UpdateVisual();
    }

    public void OnPlayerTurnEnd()
    {
        RefreshDiceSlotValidity();
        foreach(var diceSlot in diceSlotUI)
        {
            diceSlot.DeactivateDiceSlot();
            diceSlot.RemoveDiceInSlot();
        }
        UpdateVisual();
    }

    public void OnOwnerDead()
    {
        Debug.Log("??");
        UpdateVisual();

        // ĳ������ ������� ȣ��.
        OnSkillToggle?.Invoke(true);
    }

    public void OnOwnerRevive()
    {
        UpdateVisual();
    }

    private void CheckDiceSlotCount()
    {
        if(skillDataSO != null)
        {
            if( skillDataSO.diceRequirements.Count == 1)
            {
                oneDiceSlot.SetActive(true);
                diceSlotUI = oneDiceSlot.GetComponentsInChildren<SkillDiceSlotUI>();
                twoDiceSlot.SetActive(false);
            }
            else if(skillDataSO.diceRequirements.Count == 2)
            {
                oneDiceSlot.SetActive(false);
                twoDiceSlot.SetActive(true);
                diceSlotUI = twoDiceSlot.GetComponentsInChildren<SkillDiceSlotUI>();
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
        
        // �������� ��� �� ���� �ƴ�
        OnSkillToggle?.Invoke(false);

        UpdateVisual();
        
    }

    // ��ų�� ��ų ���Կ� �־������� ó�� �κ�

    public void OnSkillSlotAttach(SkillUISlot attachedSlot)
    {
        skillUISlot = attachedSlot;
        UpdateVisual();
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

    public bool CheckSkillActive()
    {
        if(skillDataSO != null && IsAttachedToSkillUISlot())
        {
            if(SkillUseLeftCount < 1 || skillUISlot.GetCharacter().stat.isDead)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        return false;
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
        if (CheckSkillActive())
        {
            
            Color characterColor = activeColors[(int)skillDataSO.CharacterType];
            skillBackgroundImage.color = characterColor;
        }
        else
        {
            Color characterColor = deactivatedColors[(int)skillDataSO.CharacterType];
            skillBackgroundImage.color = characterColor;
        }
    }
}
