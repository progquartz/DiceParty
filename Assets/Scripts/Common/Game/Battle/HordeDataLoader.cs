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

        // "HordeData" �������� ��� EnemyHordeDataSO �ҷ�����
        EnemyHordeDataSO[] allData = Resources.LoadAll<EnemyHordeDataSO>("ScriptableObject/HordeData");


        _allHordeData = allData.ToList();

        _isInitialized = true;
    }

    /// <summary>
    /// Ư�� StageType������ �����ϴ� HordeDataSO �� �ϳ� ����.
    /// </summary>
    public EnemyHordeDataSO GetTotalRandomHorde(BattleType stageType)
    {
        var filteredList = _allHordeData.Where(h => h.stageType == stageType).ToList();
        Debug.Log($"{filteredList.Count}���� �������� Ÿ�� �� �ϳ��� ���õ˴ϴ�. ����");
        foreach(EnemyHordeDataSO h in filteredList)
        {
            Debug.Log($"{h.name}�� �̸��� ������ �ֽ��ϴ�.");
        }
        if (filteredList.Count == 0)
        {
            Logger.LogWarning($"[HordeDataLoader] {stageType}�� �ش��ϴ� HordeDataSO�� �����ϴ�!");
            return null;
        }

        int randomIndex = Random.Range(0, filteredList.Count);
        if (filteredList[randomIndex] == null)
        {
            Debug.LogError("Horde List���� ������ ȣ�带 ���������� ���� null�Դϴ�.");
            return null;
        }
        return filteredList[randomIndex];
    }

    /// <summary>
    /// ���� ������ ����� Stage ������ �����ϴ� HordeDataSO �� �ϳ� ����.
    /// </summary>
    public EnemyHordeDataSO GetWeightRandomHorde(BattleType stageType)
    {
        float totalWeight = 0f;
        List<EnemyHordeDataSO> filteredList = _allHordeData.Where(h => h.stageType == stageType).ToList();
        if (filteredList.Count == 0)
        {
            Logger.LogWarning($"[HordeDataLoader] {stageType}�� �ش��ϴ� HordeDataSO�� �����ϴ�!");
            return null;
        }

        foreach ( EnemyHordeDataSO data in filteredList )
        {
            totalWeight += data.spawnWeight;
        }

        // ���� ���
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
        Logger.LogError($"[HordDataLoader] ���� ���� �������� ȣ�� �����͸� ��⸦ �����߽��ϴ�.");
        return null;
    }
}
