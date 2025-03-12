using UnityEngine;
using UnityEngine.UI;

public class SkillDiceSlotUI : MonoBehaviour
{
    [SerializeField] private SkillUI owner;
    [SerializeField] private Image diceSlotImage;

    // 현재 슬롯에 있는 Dice (없다면 null)
    [SerializeField] private Dice storedDice;
    [SerializeField] private bool isSlotUsed = false;
    [SerializeField] private bool isDiceSlotActive = false;

    [SerializeField] private Color[] activeColors;
    [SerializeField] private Color[] deactivatedColors;


    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        owner.OnSkillToggle += OnSkillSlotToggle;
    }
    private void ReleaseEvents()
    {
        owner.OnSkillToggle -= OnSkillSlotToggle;
    }


    /// <summary>
    /// 스킬 UI 활성 상태에 변화가 있을 경우에 호출됨.
    /// </summary>
    private void OnSkillSlotToggle(bool isTriggeredByDeath)
    {
        // 캐릭터가 죽어 있거나, 스킬이 비활성화되어 있을 경우 비활성화
        if (!owner.CheckSkillActive())
        {
            Debug.Log("스킬이 비활성인가 확인됨.");
            DeactivateDiceSlot();
            if(!isTriggeredByDeath)
            {
                RemoveDiceInSlot();
            }
        }
        // 단순 스킬 토글링이 발생된 경우...
        else
        {
            // 슬롯에 있는 주사위는 제거하고...
            RemoveDiceInSlot();
            // 캐릭터 스킬은 켜줌.
            ActivateDiceSlot();
        }
    }

    public void DeactivateDiceSlot()
    {
        //Logger.LogWarning($"{owner.gameObject.name} 비활성화됨.");
        isDiceSlotActive = false;
    }

    public void ActivateDiceSlot()
    {
        if(owner.CheckSkillActive() )
        {
            //Logger.LogWarning($"{owner.gameObject.name}/활성화됨/.");
            isDiceSlotActive = true;
        }
    }

    /// <summary>
    /// 주사위가 슬롯에 들어올 때(부착 시도 시) 호출될 함수
    /// </summary>
    public void OnDiceAttach(Dice dice)
    {
        // 이미 다른 주사위가 들어있다면 부착 시도 실패
        if (storedDice != null)
        {
            // 튕겨내는 판정 등은 원래 자리로 되돌리기만 하면 됨.
            Logger.Log($"이미 {gameObject.name} 슬롯에 자리가 있습니다.");
            return;
        }

        storedDice = dice;
        
        Debug.Log($"[SkillDiceSlotUI] {dice.name} 주사위가 이 슬롯에 부착됨.");


        // 주사위를 슬롯의 위치로 이동.
        dice.transform.position = transform.position;

        // 주사위의 값이 유효 조건 체크
        // 원하는 추가 조건 (숫자, 이펙트, UI 변경 등등, etc.)
        bool isDiceValid = owner.OnDiceAttach(dice, transform.GetSiblingIndex());

        // Color Checking
        if (isDiceValid)
            SetSlotColor(Color.green);
        else
            SetSlotColor(Color.red);

        // 조건이 맞을 경우 스킬 발동.
        if(isDiceValid)
        {
            owner.CheckDiceValidity();
        }
    }

    /// <summary>
    /// 슬롯에서 주사위가 나갈 때 호출됨
    /// </summary>
    public void OnDiceDetach(Dice dice)
    {
        if (storedDice == dice)
        {
            //Logger.Log($"{dice.name} 주사위가 슬롯에서 제거되었음.");
            SetSlotColor(Color.grey);
            owner.OnDiceDetach(dice, transform.GetSiblingIndex());
            storedDice = null;
        }
        else
        {
            // 혹시 모름
            Debug.Log($"[SkillDiceSlotUI] 이 슬롯에는 {dice.name}가 없거나 다른 주사위가 있습니다.");
        }

        // 필요하면 UI도 같이 초기화
        SetSlotColor(Color.white);
    }

    public void RemoveDiceInSlot()
    {
        if(storedDice != null)
        {
            storedDice.DestroySelf();
            OnDiceDetach(storedDice);
        }
    }

    public bool IsSlotAvailableToDice()
    {
        // 다이스는 활성, 활성화 상태만 받을 수 있음.
        return isDiceSlotActive && !HasDice();
    }
    /// <summary>
    /// 이 슬롯에 현재 주사위가 들어가 있는지
    /// </summary>
    public bool HasDice()
    {
        return storedDice != null;
    }

    /// <summary>
    /// 현재 슬롯에 들어있는 주사위 반환 (없으면 null)
    /// </summary>
    public Dice GetStoredDice()
    {
        return storedDice;
    }

    private void SetSlotColor(Color slotColor)
    {
        diceSlotImage.color = slotColor;
    }

    private void OnDestroy()
    {
        ReleaseEvents();
    }
}
