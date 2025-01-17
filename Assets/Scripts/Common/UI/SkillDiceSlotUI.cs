using UnityEngine;

public class SkillDiceSlotUI : MonoBehaviour
{
    // 현재 슬롯에 들어온 Dice (없다면 null)
    private Dice storedDice;

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

        // 원하는 추가 동작 (사운드, 이펙트, UI 색상 변경, etc.)

        // 주사위를 슬롯의 위치로 이동.
        dice.transform.position = transform.position;
    }

    /// <summary>
    /// 슬롯에서 주사위가 나갈 때 호출
    /// </summary>
    public void OnDiceDetach(Dice dice)
    {
        if (storedDice == dice)
        {
            Logger.Log($"{dice.name} 주사위가 슬롯에서 빠져나감.");
            storedDice = null;
        }
        else
        {
            // 혹은 무시
            Debug.Log($"[SkillDiceSlotUI] 이 슬롯에는 {dice.name}가 없거나 다른 주사위가 들어있음.");
        }

        // 필요하면 UI나 상태 초기화
        // GetComponent<Image>().color = Color.white;
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
}
