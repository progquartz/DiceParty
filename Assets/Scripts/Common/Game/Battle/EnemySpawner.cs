using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BaseEnemy enemyPrefab;
    private float deltaX = 3f;

    public BaseEnemy SpawnEnemy(EnemyDataSO enemyData, Transform parentTransform)
    {
        if (enemyData == null)
            Debug.LogError("enemyData가 null로 입력되었습니다.");
        BaseEnemy newEnemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        newEnemy.transform.parent = parentTransform;
        newEnemy.gameObject.name = enemyData.name;
        newEnemy.Init(enemyData);
        Logger.Log($"{enemyData.enemyName} 의 이름의 적을 스폰합니다.");
        BattleManager.Instance.AddEnemy(newEnemy);
        return newEnemy;
    }

    public List<BaseEnemy> SpawnEnemyList(List<EnemyDataSO> enemyDataList, Transform parentTransform)
    {
        List<BaseEnemy> spawnedEnemies = new List<BaseEnemy>();

        // 예: 간단하게 x좌표만 조금씩 움직여 스폰
        for (int i = 0; i < enemyDataList.Count; i++)
        {
            // 조절 가능하게 만들기
            Vector3 spawnPos = Vector3.zero;
            BaseEnemy newEnemy = SpawnEnemy(enemyDataList[i], parentTransform);
            spawnedEnemies.Add(newEnemy);
        }

        

        return spawnedEnemies;
    }

    public void RearrangeEnemyTransform(Transform parentTransform, List<BaseEnemy> enemies)
    {
        if(enemies.Count == 1)
        {
            return;
        }

        float startPosX = -(enemies.Count - 1) * deltaX / 2;
        for(int i = 0; i < enemies.Count; i++) 
        {
            enemies[i].transform.localPosition = new Vector3(enemies[i].transform.localPosition.x + startPosX + (deltaX * i), enemies[i].transform.localPosition.y, enemies[i].transform.localPosition.z);
        }
    }
}
