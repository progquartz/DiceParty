using UnityEngine;

public class SkillDiceSlotUI : MonoBehaviour
{
    [SerializeField] SkillUI owner;

    public void OnDiceDropped(Dice dice)
    {
        owner.UseSkill();
    }
}
