using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : BaseUI
{
    // shopitemui 들에서 아이템들을 배치하는 것을 관리.
    // 레이아웃 전체적인 프리팹자리는 이곳에 둠.
    // 판매 등 관련 ShopUI를 호출하여 결과를 처리.


    // 랜덤하게 하나의 배치 중 선택해 생성.
    [SerializeField] private GameObject[] shopLayoutLists;
    [SerializeField] private GameObject shopLayout;

    public event Action OnBuyItem;

    private void Awake()
    {
        IntantiateRandomLayout();
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        BattleManager.Instance.OnBattleStart += FinishShopping;
        MapManager.Instance.OnMoveRoom += FinishShopping;
    }

    private void ReleaseEvents()
    {
        BattleManager.Instance.OnBattleStart -= FinishShopping;
        MapManager.Instance.OnMoveRoom -= FinishShopping;
    }

    private void IntantiateRandomLayout()
    {
        int randomIndex = UnityEngine.Random.Range(0, shopLayoutLists.Length);
        shopLayout = Instantiate(shopLayoutLists[randomIndex], this.transform);
    }

    public void FinishShopping()
    {
        ReleaseEvents();
        Debug.Log("이벤트 해제");
        Close();
    }


    public bool SellItem(int price)
    {
        if(Inventory.Instance.CheckPriceAvailable(price))
        {
            OnBuyItem?.Invoke();
            return true;
        }
        return false;
    }
}
