using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HordeDataLoader : SingletonBehaviour<HordeDataLoader>
{
    [SerializeField] private List<EnemyHordeDataSO> _allHordeData = new List<EnemyHordeDataSO>();

    private bool _isInitialized = false;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if (_isInitialized) return;

        // "HordeData" 폴더에서 모든 EnemyHordeDataSO 불러오기
        EnemyHordeDataSO[] allData = Resources.LoadAll<EnemyHordeDataSO>("ScriptableObject/HordeData");


        _allHordeData = allData.ToList();

        _isInitialized = true;
    }

    /// <summary>
    /// 특정 StageType조건을 만족하는 HordeDataSO 중 하나 리턴.
    /// </summary>
    public EnemyHordeDataSO GetTotalRandomHorde(BattleType stageType)
    {
        var filteredList = _allHordeData.Where(h => h.stageType == stageType).ToList();
        Debug.Log($"{filteredList.Count}개의 스테이지 타입 중 하나가 선택됩니다. 각각");
        foreach(EnemyHordeDataSO h in filteredList)
        {
            Debug.Log($"{h.name}의 이름을 가지고 있습니다.");
        }
        if (filteredList.Count == 0)
        {
            Logger.LogWarning($"[HordeDataLoader] {stageType}에 해당하는 HordeDataSO가 없습니다!");
            return null;
        }

        int randomIndex = Random.Range(0, filteredList.Count);
        if (filteredList[randomIndex] == null)
        {
            Debug.LogError("Horde List에서 무작위 호드를 꺼내왔지만 값이 null입니다.");
            return null;
        }
        return filteredList[randomIndex];
    }

    /// <summary>
    /// 스폰 비중을 고려한 Stage 조건을 만족하는 HordeDataSO 중 하나 리턴.
    /// </summary>
    public EnemyHordeDataSO GetWeightRandomHorde(BattleType stageType)
    {
        float totalWeight = 0f;
        List<EnemyHordeDataSO> filteredList = _allHordeData.Where(h => h.stageType == stageType).ToList();
        if (filteredList.Count == 0)
        {
            Logger.LogWarning($"[HordeDataLoader] {stageType}에 해당하는 HordeDataSO가 없습니다!");
            return null;
        }

        foreach ( EnemyHordeDataSO data in filteredList )
        {
            totalWeight += data.spawnWeight;
        }

        // 비중 계산
        float randomIndex = Random.Range(0, totalWeight);
        float stackingWeight = 0f;
        foreach(EnemyHordeDataSO data in filteredList)
        {
            stackingWeight += data.spawnWeight;
            if( stackingWeight >= totalWeight )
            {
                return data;
            }
        }
        Logger.LogError($"[HordDataLoader] 비중 포함 랜덤에서 호드 데이터를 얻기를 실패했습니다.");
        return null;
    }
}
