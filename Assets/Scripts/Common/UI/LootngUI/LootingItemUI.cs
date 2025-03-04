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

    public void Init()
    {
        lootingImage.sprite = LootingItem.GetImage();
        lootingName.name = LootingItem.GetName();
        lootingName2.name = LootingItem.GetName();
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
        // ī�� ����
    }

    public string GetLore()
    {
        return "������ ī�带 ȹ���մϴ�.";
    }

    public string GetName()
    {
        return "������ ī��";
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
