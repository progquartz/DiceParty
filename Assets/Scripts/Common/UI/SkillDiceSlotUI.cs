using UnityEngine;

public class SkillDiceSlotUI : MonoBehaviour
{
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

        // ���ϴ� �߰� ���� (����, ����Ʈ, UI ���� ����, etc.)

        // �ֻ����� ������ ��ġ�� �̵�.
        dice.transform.position = transform.position;
    }

    /// <summary>
    /// ���Կ��� �ֻ����� ���� �� ȣ��
    /// </summary>
    public void OnDiceDetach(Dice dice)
    {
        if (storedDice == dice)
        {
            Logger.Log($"{dice.name} �ֻ����� ���Կ��� ��������.");
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
}
