using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public int Hp;
    public int maxHp;
    public int ArmourStack;

    public bool isDead = false;


    // ¹öÇÁ ºÎºÐ.
    
    public int StrengthStack; // Èû
    public int PassionStack; // ¿­Á¤
    public int ThornStack; // °¡½Ã°©¿Ê
    public int TauntStack; // µµ¹ß

    public int CleanseStack; // Á¤È­



    // µð¹öÇÁ ºÎºÐ

    // µ¶ / È­¿°
    public int PoisonStack;
    public int FireStack;


    public int ConfuseStack; // È¥¶õ
    public int WeakenStack; // ¼è¾à

    // ½ºÅÏ
    public int StunnedStack;
    
    public void SetStat(BaseStat stat)
    {
        this.Hp = stat.Hp;
        this.maxHp = stat.maxHp;
        this.ArmourStack = stat.ArmourStack;

        this.isDead = stat.isDead;
    }
}
