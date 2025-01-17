using UnityEngine;

public class SkillDiceSlotUI : MonoBehaviour
{
    [SerializeField] SkillUI owner;

    public void OnDiceDropped(Dice dice)
    {
        owner.UseSkill();
    }

    public bool CheckAvailability(Dice dice)
    {
        return owner.CheckDiceAvailability(dice);
    }
}
