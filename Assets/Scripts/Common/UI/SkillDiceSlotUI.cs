using UnityEngine;
using UnityEngine.UI;

public class SkillDiceSlotUI : MonoBehaviour
{
    [SerializeField] private SkillUI owner;
    [SerializeField] private Image diceSlotImage;

    // 현재 슬롯에 들어온 Dice (없다면 null)
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

        BattleManager.Instance.OnPlayerTurnEnd += RemoveDiceInSlot;
        BattleManager.Instance.OnPlayerTurnEnd += DeactivateDiceSlot;

        BattleManager.Instance.OnPlayerTurnStart += ActivateDiceSlot;
    }
    private void ReleaseEvents()
    {
        owner.OnSkillToggle -= OnSkillSlotToggle;

        BattleManager.Instance.OnPlayerTurnEnd -= RemoveDiceInSlot;
        BattleManager.Instance.OnPlayerTurnEnd -= DeactivateDiceSlot;

        BattleManager.Instance.OnPlayerTurnStart -= ActivateDiceSlot;
    }


    /// <summary>
    /// 스킬 UI 사용 상태에 변화가 생길 경우에 호출됨.
    /// </summary>
    private void OnSkillSlotToggle(bool isTriggeredByDeath)
    {
        // 캐릭터가 사망 상태이거나, 스킬이 모두 사용되었을 경우 비활성화
        if (!owner.CheckSkillActive())
        {
            Debug.Log("스킬이 사망인것 확인됨.");
            DeactivateDiceSlot();
            if(!isTriggeredByDeath)
            {
                RemoveDiceInSlot();
            }
        }
        // 단순 스킬 사용으로 토글된 경우...
        else
        {
            // 슬롯에 있는 주사위를 버리고...
            RemoveDiceInSlot();
            // 캐릭터 스킬이 열림.
            ActivateDiceSlot();
        }
    }

    private void DeactivateDiceSlot()
    {
        //Logger.LogWarning($"{owner.gameObject.name} 비활성화됨.");
        isDiceSlotActive = false;
    }

    private void ActivateDiceSlot()
    {
        if(owner.CheckSkillActive() )
        {
            //Logger.LogWarning($"{owner.gameObject.name}/활성화됨/.");
            isDiceSlotActive = true;
        }
    }

    /// <summary>
    /// 주사위가 슬롯에 들어올 때(드롭 성공 시) 호출될 함수
    /// </summary>
    public void OnDiceAttach(Dice dice)
    {
        // 이미 다른 주사위가 들어있다면 어떻게 할지 결정
        if (storedDice != null)
        {
            // 튕겨내기 또는 원래 슬롯 자리로 되돌려놓기 필요.
            Logger.Log($"이미 {gameObject.name} 슬롯 자리는 사용되고 있습니다.");
            return;
        }

        storedDice = dice;
        
        Debug.Log($"[SkillDiceSlotUI] {dice.name} 주사위를 이 슬롯에 보관.");


        // 주사위를 슬롯의 위치로 이동.
        dice.transform.position = transform.position;

        // 주사위의 제약 조건 여부 체크
        // 원하는 추가 동작 (사운드, 이펙트, UI 색상 변경, etc.)
        bool isDiceValid = owner.OnDiceAttach(dice, transform.GetSiblingIndex());

        // Color Checking
        if (isDiceValid)
            SetSlotColor(Color.green);
        else
            SetSlotColor(Color.red);

        // 조건이 맞을 경우 스킬 실행.
        if(isDiceValid)
        {
            owner.CheckDiceValidity();
        }
    }

    /// <summary>
    /// 슬롯에서 주사위가 나갈 때 호출
    /// </summary>
    public void OnDiceDetach(Dice dice)
    {
        if (storedDice == dice)
        {
            Logger.Log($"{dice.name} 주사위가 슬롯에서 빠져나감.");
            SetSlotColor(Color.grey);
            owner.OnDiceDetach(dice, transform.GetSiblingIndex());
            storedDice = null;
        }
        else
        {
            // 혹은 무시
            Debug.Log($"[SkillDiceSlotUI] 이 슬롯에는 {dice.name}가 없거나 다른 주사위가 들어있음.");
        }

        // 필요하면 UI나 상태 초기화
        SetSlotColor(Color.white);
    }

    private void RemoveDiceInSlot()
    {
        if(storedDice != null)
        {
            storedDice.DestroySelf();
            OnDiceDetach(storedDice);
        }
    }

    public bool IsSlotAvailableToDice()
    {
        // 다이스가 없고, 활성화 상태면 사용 가능.
        return isDiceSlotActive && !HasDice();
    }
    /// <summary>
    /// 이 슬롯이 현재 주사위를 가지고 있는지
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
