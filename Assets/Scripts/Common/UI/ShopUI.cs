using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    // shopitemui 기반의 아이템들을 배치하는 공간 제작.
    // 각각의 객체들은 시작하자마자 이곳에 등록.
    // 판매 될 때에 ShopUI를 호출하여 계산을 처리.


    // 랜덤하게 하나가 배치 될 아이템 공간.
    [SerializeField] GameObject[] shopLayoutLists;

    public event Action OnBuyItem;

    public bool SellItem(int price)
    {
        if(CheckPriceAvailable(price))
        {
            OnBuyItem?.Invoke();
            return true;
        }
        return false;
    }

    private bool CheckPriceAvailable(int price)
    {
        if (Inventory.Instance.gold >= price)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
