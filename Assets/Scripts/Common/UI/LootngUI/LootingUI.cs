using UnityEngine;

public class LootingUI : BaseUI
{
    [SerializeField] private Transform LootingItemParent;
    
    [SerializeField] private GameObject LootingItemPrefab;


    private void Awake()
    {
        RegisterEvents();
        LootingSetup();
    }

    private void LootingSetup()
    {
        LootingProbTable lootingProbTable = LootingManager.Instance.GetLootingTable();

        int cardCount = GetLootCount(lootingProbTable.CardPercent);
        int treasureCount = GetLootCount(lootingProbTable.TreasurePercent);
        int potionCount = GetLootCount(lootingProbTable.PotionPercent);
        int diceCount = GetLootCount(lootingProbTable.DicePercent);

        for(int i = 0; i < cardCount; i++)
        {
            GameObject spawnedLoot = Instantiate(LootingItemPrefab);
            spawnedLoot.transform.parent = LootingItemParent;
            LootingItem item = spawnedLoot.AddComponent<LootingCard>();
            spawnedLoot.GetComponent<LootingItemUI>().LootingItem = item;
            spawnedLoot.GetComponent<LootingItemUI>().Init();
        }

        for(int i = 0;i < treasureCount; i++)
        {

        }

        for (int i = 0;i < potionCount ; i++) 
        { 

        }

        for(int i = 0; i < diceCount ; i++)
        {

        }

        /// 설정 끝나면...
    }

    private int GetLootCount(int percent)
    {
        int lootCount = percent / 100;
        int leftPercent = percent % 100;

        int randomNum = Random.Range(0, 100);
        if(randomNum <= leftPercent) 
        {
            return lootCount + 1;
        }
        else
        {
            return lootCount;
        }
    }
    private void RegisterEvents()
    {
        BattleManager.Instance.OnBattleStart += FinishLooting;
        MapManager.Instance.OnMoveRoom += FinishLooting;
    }

    private void ReleaseEvents()
    {
        BattleManager.Instance.OnBattleStart -= FinishLooting;
        MapManager.Instance.OnMoveRoom -= FinishLooting;
    }

    private void SetUpTreasure()
    {

    }


    public void FinishLooting()
    {
        ReleaseEvents();
        Debug.Log("이벤트 연결");
        Close();
    }


}
