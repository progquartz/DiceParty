using UnityEngine;
using TMPro;
using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class SkillUI : MonoBehaviour
{
    
    [Header("스킬 데이터")]
    [SerializeField] private SkillDataSO skillDataSO;

    [Header("주사위 슬롯 증명성 여부")]
    [SerializeField] private bool[] diceSlotValidity;

    [Header("등록된 스킬 슬롯")]
    [SerializeField] private SkillUISlot skillUISlot;

    [Header("UI 표시용 텍스트")]
    
    [SerializeField] private TMP_Text skillNameText;
    [SerializeField] private TMP_Text skillLoreText;
    [SerializeField] private Image skillBackgroundImage;

    [Header("주사위 조건 텍스트")]
    [SerializeField] private List<TMP_Text> diceNeedTextOneSlot;
    [SerializeField] private List<TMP_Text> diceNeedTextTwoSlot;

    [Header("주사위 슬롯 1개짜리")]
    [SerializeField] private GameObject oneDiceSlot;
    [Header("주사위 슬롯 2개짜리")]
    [SerializeField] private GameObject twoDiceSlot;


    // 스킬 실행 클래스
    private SkillExecutor skillExecutor;

    public event Action<int> OnSkillUse;
    public int SkillUseLeftCount;

    

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
            // Dice 조건 맞는지 여부를 초기화.
            diceSlotValidity = new bool[skillDataSO.diceRequirements.Count];
        }
    }

    public void OnBattleStart()
    {
        DestroyIfNotAttached();
    }
    public void OnPlayerTurnStart()
    {
        // count 초기화
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

    /// <summary>
    /// 스킬 사용
    /// </summary>
    public void UseSkill(BaseTarget caller)
    {
        if(!IsAttachedToSkillUISlot())
        {
            Logger.LogError("[SkillUI]슬롯에 등록되지 않은 스킬이 사용되고 있습니다!");
            return;
        }

        // 주사위 상태 초기화.
        RefreshDiceSlotValidity();

        // 사용 개수 차감.
        SkillUseLeftCount--;

        skillExecutor.UseSkill(skillDataSO, caller);
        OnSkillUse?.Invoke(SkillUseLeftCount);
        UpdateVisual();
        
    }

    // 스킬을 스킬 슬롯에 넣었을때의 처리 부분

    public void OnSkillSlotAttach(SkillUISlot attachedSlot)
    {
        skillUISlot = attachedSlot;
    }

    public void OnSkillSlotDetach()
    {
        skillUISlot = null;
    }

    // 주사위 슬롯 내의 주사위 처리 부분

    /// <summary>
    /// 주사위가 스킬의 슬롯에 들어와 주사위의 조건을 체크.
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

        // 모든 주사위 슬롯이 조건에 맞아 스킬을 사용.
        if (isAllDiceValid)
        {
            Logger.Log($"{diceSlotValidity.Length} 개의 슬롯을 가진 {skillDataSO.skillName} 스킬이 조건에 맞아 실행됩니다.");
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


    // 내부 데이터 처리

    public void SetSkillData(SkillDataSO newSkillData)
    {
        skillDataSO = newSkillData;
        SkillUseLeftCount = newSkillData.skillUseCount;
        // 필요 시 UI도 갱신
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
        // 추후에 애니메이션 필요..?
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
