using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public int Hp;
    public int maxHp;
    public int ArmourStack;

    public bool isDead = false;


    // 버프 부분.
    
    public int StrengthStack; // 힘
    
    public int ThornStack; // 가시갑옷
    public int TauntStack; // 도발

    public int ImmuneStack; // 정화

    // 버프 증가 버프
    public int fortifyStack; // 방벽
    public int PassionStack; // 열정
    public int RegenStack;

    // 디버프 부분

    // 독 / 화염
    public int PoisonStack; // 독
    public int FireStack; // 화염


    public int ConfuseStack; // 혼란
    public int WeakenStack; // 쇠약
    public int WitherStack; // 부패

    
    public int StunnedStack; // 스턴

    public void SetStat(BaseStat stat)
    {
        this.Hp = stat.Hp;
        this.maxHp = stat.maxHp;
        this.ArmourStack = stat.ArmourStack;

        this.isDead = stat.isDead;


        this.StrengthStack = stat.StrengthStack;

        this.ThornStack  = stat.ThornStack;
        this.TauntStack  = stat.TauntStack;

        this.ImmuneStack = stat.ImmuneStack;

        this.fortifyStack = stat.fortifyStack;
        this.PassionStack = stat.PassionStack;
        this.RegenStack = stat.RegenStack;  

        this.PoisonStack = stat.PoisonStack;
        this.FireStack = stat.FireStack;


        this.ConfuseStack = stat.ConfuseStack;
        this.WeakenStack = stat.WeakenStack;
        this.WitherStack = stat.WitherStack;
    }
}
