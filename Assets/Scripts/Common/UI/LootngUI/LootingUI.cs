using UnityEngine;

public class LootingUI : BaseUI
{
    [SerializeField] private Transform LootingItemParent;
    
    [SerializeField] private GameObject LootingItemPrefab;

    [SerializeField] private int lootingUICount;

    public override void Init(Transform canvas)
    {
        base.Init(canvas);
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

        cardCount = 1;
        Debug.Log($"{cardCount} '' {treasureCount} '' {potionCount} '' {diceCount}");
        lootingUICount = 1 + cardCount + treasureCount + potionCount + diceCount;

        // 골드.
        GameObject spawnedGold = Instantiate(LootingItemPrefab);
        spawnedGold.transform.parent = LootingItemParent;
        spawnedGold.transform.localScale = Vector3.one;
        LootingItem gold = spawnedGold.AddComponent<LootingGold>();
        gold.LoadData();
        spawnedGold.GetComponent<LootingItemUI>().LootingItem = gold;
        spawnedGold.GetComponent<LootingItemUI>().Init(this);

        // 카드
        for (int i = 0; i < cardCount; i++)
        {
            GameObject spawnedLoot = Instantiate(LootingItemPrefab);
            spawnedLoot.transform.parent = LootingItemParent;
            spawnedLoot.transform.localScale = Vector3.one; 
            LootingItem item = spawnedLoot.AddComponent<LootingCard>();
            spawnedLoot.GetComponent<LootingItemUI>().LootingItem = item;
            
            spawnedLoot.GetComponent<LootingItemUI>().Init(this);
            
        }

        for(int i = 0;i < treasureCount; i++)
        {

        }

        for (int i = 0;i < potionCount ; i++) 
        {
            GameObject spawnedLoot = Instantiate(LootingItemPrefab);
            spawnedLoot.transform.parent = LootingItemParent;
            spawnedLoot.transform.localScale = Vector3.one;
            LootingItem potion = spawnedLoot.AddComponent<LootingPotion>();
            potion.LoadData();
            spawnedLoot.GetComponent<LootingItemUI>().LootingItem = potion;
            spawnedLoot.GetComponent<LootingItemUI>().Init(this);
        }

        for(int i = 0; i < diceCount ; i++)
        {

        }

        /// 보상 세팅...
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

    public void OnClickLootingItem()
    {
        lootingUICount--;
        if(lootingUICount == 0)
        {
            FinishLooting();
        }
    }

    public void OnClickSkipButton()
    {
        FinishLooting();
    }

    public void FinishLooting()
    {
        ReleaseEvents();
        Debug.Log("이벤트 해제");
        Close();
    }
}
