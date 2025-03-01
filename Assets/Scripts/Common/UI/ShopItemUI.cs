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
    [SerializeField] private TMP_Text itemPriceText;


    private void Awake()
    {
        owner = GetComponentInParent<ShopUI>();
        SpawnCard();

    }

    private void SpawnCard()
    {

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
            buyButton.SetActive(false); // ������.
            isItemSold = true;
            // ī�� �κ��̶��...
        }
        else
        {
            // ���� �����մϴ� �޼���.
            Debug.Log("�� �������� �������� ������ �� �����ϴ�.");
        }
    }

    private void UpdateVisual()
    {
        if (isItemSold)
        {

        }
        else
        {

        }
    }

    private void CalculateSellingPrice()
    {
        SellingPrice = (int)(OriginalPrice * (1.0f - Inventory.Instance.salesPercent));
    }

}
