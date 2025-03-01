using System;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ShopItemUI : MonoBehaviour
{
    [SerializeField] private ShopUI owner;
    

    public int OriginalPrice;
    public int SellingPrice;
    private bool isItemSold = false;


    [SerializeField] private GameObject buyButton;
    [SerializeField] private Transform cardHolder;
    [SerializeField] private Transform skillUICard;
    [SerializeField] private TMP_Text itemPriceText;


    private void Awake()
    {
        owner = GetComponentInParent<ShopUI>();
        int price = SpawnCard();
        OriginalPrice = price;

    }

    private int SpawnCard()
    {
        LootingCard stageSkillDataSO = LootingManager.Instance.GetRandomLootingCard(MapManager.Instance.currentStageNum);
        SkillUI skillUi = LootingManager.Instance.SkillUiSpawner.SpawnSkillUI(stageSkillDataSO.lootingSkillDataSO);
        skillUi.transform.SetParent(cardHolder);
        skillUi.transform.localPosition = Vector3.zero;
        skillUi.transform.localScale = Vector3.one;
        skillUICard = skillUi.transform;
        return stageSkillDataSO.lootPower;
    }


    private void Update()
    {
        CalculateSellingPrice();
        UpdateVisual();
    }


    public void OnClickBuyButton()
    {
        if(isItemSold)
        {
            return;
        }

        bool isItemAvailableToBuy = owner.SellItem(SellingPrice);
        if (isItemAvailableToBuy)
        {
            buyButton.SetActive(false); // 꺼지기.
            isItemSold = true;

            // 카드 부분이라면...
            skillUICard.transform.SetParent(Inventory.Instance.cardParent);
        }
        else
        {
            // 돈이 부족합니다 메세지.
            Debug.Log("돈 부족으로 아이템을 구매할 수 없습니다.");
        }
    }

    private void UpdateVisual()
    {
        if (isItemSold)
        {
            itemPriceText.text = "판매 완료";
        }
        else
        {
            itemPriceText.text = SellingPrice.ToString() + "G";
        }
    }

    private void CalculateSellingPrice()
    {
        SellingPrice = (int)(OriginalPrice * (1.0f - Inventory.Instance.salesPercent));
    }

}
