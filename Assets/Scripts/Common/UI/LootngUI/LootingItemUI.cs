using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootingItemUI : MonoBehaviour
{
    public Image lootingImage;

    public TMP_Text lootingName;
    public TMP_Text lootingName2;
    public TMP_Text lootingLore;

    public LootingItem LootingItem;

    private LootingUI owner;

    public void OnClickButton()
    {
        LootingItem.GetItem();
        owner.OnClickLootingItem();
        // 애니메이션, 각 효과 등...
        Destroy(this.gameObject);
    }

    public void Init(LootingUI owner)
    {
        this.owner = owner;
        lootingImage.sprite = LootingItem.GetImage();
        lootingName.text = LootingItem.GetName();
        lootingName2.text = LootingItem.GetName();
        lootingLore.text = LootingItem.GetLore();
        LootingItem.InitializeLoot();
    }
}

public interface LootingItem
{
    public void GetItem();
    public string GetName();
    public string GetLore();

    public void InitializeLoot();

    public Sprite GetImage();
}

public class LootingCard : MonoBehaviour, LootingItem
{
    public Sprite GetImage()
    {
        return LootingManager.Instance.LootingDataBase.LootingCardImage;
    }

    public void GetItem()
    {
        // 카드 선택
        UIManager.Instance.OpenUI<CardSelectLootUI>(new BaseUIData
        {
            ActionOnShow = () => { Debug.Log("카드 선택 UI 열림."); },
            ActionOnClose = () => { Debug.Log("카드 선택 UI 닫힘."); }
        });
    }

    public string GetLore()
    {
        return new string("새로운 카드를 획득합니다.");
    }

    public string GetName()
    {
        return new string("새로운 카드");
    }

    public void InitializeLoot()
    {

    }
}


public class LootingPotion : MonoBehaviour, LootingItem
{
    public Sprite GetImage()
    {
        throw new System.NotImplementedException();
    }

    public void GetItem()
    {
        throw new System.NotImplementedException();
    }

    public string GetLore()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        throw new System.NotImplementedException();
    }

    public void InitializeLoot()
    {
        throw new System.NotImplementedException();
    }
}

public class LootingTreasure : MonoBehaviour, LootingItem
{
    public Sprite GetImage()
    {
        throw new System.NotImplementedException();
    }

    public void GetItem()
    {
        throw new System.NotImplementedException();
    }

    public string GetLore()
    {
        throw new System.NotImplementedException();
    }

    public string GetName()
    {
        throw new System.NotImplementedException();
    }

    public void InitializeLoot()
    {
        throw new System.NotImplementedException();
    }
}
