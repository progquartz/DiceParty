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
    [Header("스킬 데이터")]
    [SerializeField] private SkillDataSO skillDataSO;
    public CharacterType CharacterType { get { return skillDataSO.CharacterType; } }

    [Header("주사위 조건 만족도 체크")]
    [SerializeField] private bool[] diceSlotValidity;

    [Header("등록된 스킬 슬롯")]
    [SerializeField] public SkillUISlot skillUISlot;
    private SkillUISlot currentSlot = null;

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

    [Header("주사위 슬롯 UI")]
    [SerializeField] private SkillDiceSlotUI[] diceSlotUI;

    [SerializeField] private Color[] activeColors;
    [SerializeField] private Color[] deactivatedColors;

    // 스킬 실행 클래스
    private SkillExecutor skillExecutor;

    public event Action<bool> OnSkillToggle;
    public int SkillUseLeftCount;

    public void Init(SkillDataSO skillData)
    {
        SetSkillData(skillData);
        // 필요하다면 싱글톤이나 DI 등을 이용해도 됨
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
            // Dice 조건 맞는지 여부를 초기화
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
        // count 초기화
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

        // 캐릭터가 죽었음을 호출
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
                Logger.LogError($"{skillDataSO.skillName} 이름을 가진 스킬은 비정상적인 개수의 주사위 조건 설정이 필요로 합니다.");
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
    /// 스킬 발동
    /// </summary>
    public void UseSkill(BaseTarget caller)
    {
        if(!IsAttachedToSkillUISlot())
        {
            Logger.LogError("[SkillUI]슬롯에 등록되지 않은 스킬이 발동이 됩니다!");
            return;
        }

        // 주사위 조건 초기화
        RefreshDiceSlotValidity();

        // 사용 횟수 감소
        SkillUseLeftCount--;

        skillExecutor.UseSkill(skillDataSO, caller);
        
        // 토글링은 죽은 게 아님
        OnSkillToggle?.Invoke(false);

        UpdateVisual();
    }

    // 스킬이 스킬 슬롯에 들어갔을때의 처리 부분

    public void OnSkillSlotAttach(SkillUISlot attachedSlot)
    {
        skillUISlot = attachedSlot;
        UpdateVisual();
    }

    public void OnSkillSlotDetach()
    {
        skillUISlot = null;
    }

    // 주사위 조건 관련 주사위 처리 부분

    /// <summary>
    /// 주사위가 스킬의 슬롯에 맞는 주사위인지 체크
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

        // 모든 주사위가 유효한지 체크
        if (isAllDiceValid)
        {
            Logger.Log($"{diceSlotValidity.Length} 개의 주사위가 모두 유효합니다. {skillDataSO.skillName} 스킬을 사용합니다.");
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

    // 스킬 데이터 설정

    public void SetSkillData(SkillDataSO newSkillData)
    {
        skillDataSO = newSkillData;
        SkillUseLeftCount = newSkillData.skillUseCount;
        // UI 갱신
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
        // 모든 참조가 해제되었는지 확인
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
