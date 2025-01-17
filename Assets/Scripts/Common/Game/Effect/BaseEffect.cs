using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect 
{

    // Ÿ�� ������ �޾ƿ� ������, ���⿡ ������� �ϴ� Effect�� ��ӹ޾Ƽ� �ϴ� ������.
    // Ÿ���� ��� �κ���, SkillDataSO���� �޾ƿ� ������, �̸� ������� �۵�.
    // ȿ�� / ������� ���� SkillDataSO�� ���� ����.
    public abstract void Effect(List<BaseTarget> targets, int strength1, int strength2);
}
