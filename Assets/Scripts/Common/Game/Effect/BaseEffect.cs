using System.Collections.Generic;
using UnityEngine;

public class BaseEffect : MonoBehaviour
{

    // Ÿ�� ������ �޾ƿ� ������, ���⿡ ������� �ϴ� Effect�� ��ӹ޾Ƽ� �ϴ� ������.
    // Ÿ���� ��� �κ���, SkillDataSO���� �޾ƿ� ������, �̸� ������� �۵�.
    // ȿ�� / ������� ���� SkillDataSO�� ���� ����.
    public virtual void Effect(List<BaseTarget> targets, int strength)
    {

    }
}
