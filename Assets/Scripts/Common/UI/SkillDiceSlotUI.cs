using UnityEngine;
using UnityEngine.UI;

public class SkillDiceSlotUI : MonoBehaviour
{
    [SerializeField] private SkillUI owner;
    [SerializeField] private Image diceSlotImage;

    // ���� ���Կ� ���� Dice (���ٸ� null)
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
    /// ��ų UI ��� ���¿� ��ȭ�� ���� ��쿡 ȣ���.
    /// </summary>
    private void OnSkillSlotToggle(bool isTriggeredByDeath)
    {
        // ĳ���Ͱ� ��� �����̰ų�, ��ų�� ��� ���Ǿ��� ��� ��Ȱ��ȭ
        if (!owner.CheckSkillActive())
        {
            Debug.Log("��ų�� ����ΰ� Ȯ�ε�.");
            DeactivateDiceSlot();
            if(!isTriggeredByDeath)
            {
                RemoveDiceInSlot();
            }
        }
        // �ܼ� ��ų ������� ��۵� ���...
        else
        {
            // ���Կ� �ִ� �ֻ����� ������...
            RemoveDiceInSlot();
            // ĳ���� ��ų�� ����.
            ActivateDiceSlot();
        }
    }

    private void DeactivateDiceSlot()
    {
        //Logger.LogWarning($"{owner.gameObject.name} ��Ȱ��ȭ��.");
        isDiceSlotActive = false;
    }

    private void ActivateDiceSlot()
    {
        if(owner.CheckSkillActive() )
        {
            //Logger.LogWarning($"{owner.gameObject.name}/Ȱ��ȭ��/.");
            isDiceSlotActive = true;
        }
    }

    /// <summary>
    /// �ֻ����� ���Կ� ���� ��(��� ���� ��) ȣ��� �Լ�
    /// </summary>
    public void OnDiceAttach(Dice dice)
    {
        // �̹� �ٸ� �ֻ����� ����ִٸ� ��� ���� ����
        if (storedDice != null)
        {
            // ƨ�ܳ��� �Ǵ� ���� ���� �ڸ��� �ǵ������� �ʿ�.
            Logger.Log($"�̹� {gameObject.name} ���� �ڸ��� ���ǰ� �ֽ��ϴ�.");
            return;
        }

        storedDice = dice;
        
        Debug.Log($"[SkillDiceSlotUI] {dice.name} �ֻ����� �� ���Կ� ����.");


        // �ֻ����� ������ ��ġ�� �̵�.
        dice.transform.position = transform.position;

        // �ֻ����� ���� ���� ���� üũ
        // ���ϴ� �߰� ���� (����, ����Ʈ, UI ���� ����, etc.)
        bool isDiceValid = owner.OnDiceAttach(dice, transform.GetSiblingIndex());

        // Color Checking
        if (isDiceValid)
            SetSlotColor(Color.green);
        else
            SetSlotColor(Color.red);

        // ������ ���� ��� ��ų ����.
        if(isDiceValid)
        {
            owner.CheckDiceValidity();
        }
    }

    /// <summary>
    /// ���Կ��� �ֻ����� ���� �� ȣ��
    /// </summary>
    public void OnDiceDetach(Dice dice)
    {
        if (storedDice == dice)
        {
            Logger.Log($"{dice.name} �ֻ����� ���Կ��� ��������.");
            SetSlotColor(Color.grey);
            owner.OnDiceDetach(dice, transform.GetSiblingIndex());
            storedDice = null;
        }
        else
        {
            // Ȥ�� ����
            Debug.Log($"[SkillDiceSlotUI] �� ���Կ��� {dice.name}�� ���ų� �ٸ� �ֻ����� �������.");
        }

        // �ʿ��ϸ� UI�� ���� �ʱ�ȭ
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
        // ���̽��� ����, Ȱ��ȭ ���¸� ��� ����.
        return isDiceSlotActive && !HasDice();
    }
    /// <summary>
    /// �� ������ ���� �ֻ����� ������ �ִ���
    /// </summary>
    public bool HasDice()
    {
        return storedDice != null;
    }

    /// <summary>
    /// ���� ���Կ� ����ִ� �ֻ��� ��ȯ (������ null)
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
