using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BaseEnemy enemyPrefab;

    public BaseEnemy SpawnEnemy(EnemyDataSO enemyData, Transform parentTransform)
    {
        BaseEnemy newEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        newEnemy.transform.parent = parentTransform;
        newEnemy.Init(enemyData);
        BattleManager.Instance.RegisterTarget(newEnemy);
        return newEnemy;
    }

    public List<BaseEnemy> SpawnEnemyList(List<EnemyDataSO> enemyDataList, Transform parentTransform)
    {
        List<BaseEnemy> spawnedEnemies = new List<BaseEnemy>();

        // ��: �����ϰ� x��ǥ�� ���ݾ� ������ ����
        for (int i = 0; i < enemyDataList.Count; i++)
        {
            // ���� �����ϰ� �����
            Vector3 spawnPos = Vector3.zero; 
            BaseEnemy newEnemy = SpawnEnemy(enemyDataList[i], parentTransform);
            spawnedEnemies.Add(newEnemy);
        }

        return spawnedEnemies;
    }
}
