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
    public int PassionStack; // ����
    public int ThornStack; // ���ð���
    public int TauntStack; // ����

    public int CleanseStack; // ��ȭ



    // ����� �κ�

    // �� / ȭ��
    public int PoisonStack;
    public int FireStack;


    public int ConfuseStack; // ȥ��
    public int WeakenStack; // ���

    // ����
    public int StunnedStack;
    
    public void SetStat(BaseStat stat)
    {
        this.Hp = stat.Hp;
        this.maxHp = stat.maxHp;
        this.ArmourStack = stat.ArmourStack;

        this.isDead = stat.isDead;
    }
}
