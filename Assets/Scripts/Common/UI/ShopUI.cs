using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    // shopitemui ����� �����۵��� ��ġ�ϴ� ���� ����.
    // ������ ��ü���� �������ڸ��� �̰��� ���.
    // �Ǹ� �� ���� ShopUI�� ȣ���Ͽ� ����� ó��.


    // �����ϰ� �ϳ��� ��ġ �� ������ ����.
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
