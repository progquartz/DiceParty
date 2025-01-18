using UnityEngine;
using UnityEngine.UI;

public class SkillDiceSlotUI : MonoBehaviour
{
    [SerializeField] private SkillUI owner;
    [SerializeField] private Image diceSlotImage;

    // ���� ���Կ� ���� Dice (���ٸ� null)
    private Dice storedDice;
    

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

        if (isDiceValid)
        {
            // ���̽��� ������ ������ Ǫ���� ����.
            SetSlotColor(Color.green);
        }
        else
        {
            // ���̽��� ���� ������ �Ӱ� ����.
            SetSlotColor(Color.red);
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
            storedDice = null;
        }
        else
        {
            // Ȥ�� ����
            Debug.Log($"[SkillDiceSlotUI] �� ���Կ��� {dice.name}�� ���ų� �ٸ� �ֻ����� �������.");
        }

        // �ʿ��ϸ� UI�� ���� �ʱ�ȭ
        // GetComponent<Image>().color = Color.white;
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
}
