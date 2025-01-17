using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetAllEnemy : BaseTargetOption
{
    public override List<BaseTarget> GetTarget()
    {
        
        return FindAllEnemy();
    }

    [Obsolete("��� �� ã�ƿ��� ��� �ϼ��ȵ�.")]
    private List<BaseTarget> FindAllEnemy()
    {
        // ��: �� ���� ��� BaseEnemy�� ã��
        BaseEnemy[] enemies = GameObject.FindObjectsOfType<BaseEnemy>();
        return new List<BaseTarget>(enemies);
    }
}
