using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public int Hp;
    public int maxHp;
    public int Armour;
    public int AdditionalDamageStack;
    public int AdditionalHealStack;

    // µ¶ / È­¿°
    public int PoisonStack;
    public int FireStack;

    // Èû / ¼è¾à
    public int StrengthStack;
    public int WeakenStack;

    // ½ºÅÏ
    public int StunnedStack;
    

    public bool isDead = false;

    public void SetStat(BaseStat stat)
    {
        this.Hp = stat.Hp;
        this.maxHp = stat.maxHp;
        this.Armour = stat.Armour;
        this.AdditionalDamageStack = stat.AdditionalDamageStack;
        this.AdditionalHealStack = stat.AdditionalHealStack;

        this.isDead = stat.isDead;
    }
}
