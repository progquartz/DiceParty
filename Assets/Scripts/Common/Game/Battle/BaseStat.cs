using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class BaseStat
{
    public int Hp;
    public int maxHp;
    public int ArmourStack;

    public bool isDead = false;


    // ���� �κ�.
    
    public int StrengthStack; // ��
    
    public int ThornStack; // ���ð���
    public int TauntStack; // ����

    public int ImmuneStack; // ��ȭ

    // ���� ���� ����
    public int fortifyStack; // �溮
    public int PassionStack; // ����
    public int RegenStack;

    // ����� �κ�

    // �� / ȭ��
    public int PoisonStack; // ��
    public int FireStack; // ȭ��


    public int ConfuseStack; // ȥ��
    public int WeakenStack; // ���
    public int WitherStack; // ����

    
    public int StunnedStack; // ����

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
