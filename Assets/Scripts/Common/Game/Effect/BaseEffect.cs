using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect 
{
    // 타겟 정보를 받아와 실행하고, 여기에 구현하지 않는 Effect는 상속받아서 하는 구조임.
    // 타겟의 모든 부분은, SkillDataSO에서 받아와 실행함, 이름 정도만 작동.
    // 효과 / 데미지는 모두 SkillDataSO에 값이 있음.
    public abstract void Effect(List<BaseTarget> targets, BaseTarget caller,  int strength1, int strength2);
}
