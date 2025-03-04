using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : BaseUI
{
    // shopitemui ����� �����۵��� ��ġ�ϴ� ���� ����.
    // ������ ��ü���� �������ڸ��� �̰��� ���.
    // �Ǹ� �� ���� ShopUI�� ȣ���Ͽ� ����� ó��.


    // �����ϰ� �ϳ��� ��ġ �� ������ ����.
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
        Debug.Log("�̺�Ʈ ����");
        Close();
    }


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
