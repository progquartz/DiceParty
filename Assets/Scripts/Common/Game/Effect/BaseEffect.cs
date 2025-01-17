using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEffect 
{

    // 타겟 데이터 받아온 다음에, 여기에 기반으로 하는 Effect를 상속받아서 하는 것으로.
    // 타겟을 잡는 부분은, SkillDataSO에서 받아온 다음에, 이를 기반으로 작동.
    // 효과 / 대상으로 나눈 SkillDataSO로 만들 예정.
    public abstract void Effect(List<BaseTarget> targets, int strength1, int strength2);
}
