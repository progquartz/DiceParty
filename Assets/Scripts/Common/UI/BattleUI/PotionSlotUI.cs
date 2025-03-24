using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PotionSlotUI : MonoBehaviour
{
    [SerializeField] private GameObject PotionUseUI;
    [SerializeField] private Image potionSprite;

    [SerializeField] private TextMeshProUGUI potionNameText;
    [SerializeField] private TextMeshProUGUI potionLoreText;
    // 포션 버튼의 이미지
    [SerializeField] private Button slotButton;
    [SerializeField] private Image slotImage;
    

    [SerializeField] private bool isUseUIActivate = false;
    public int index;

    public void UpdateLoreTexts(PotionDataSO potionData)
    {
        if(potionData != null)
        {
            potionNameText.text = potionData.name;
            potionLoreText.text = potionData.lore;
        }
    }

    public void UpdateSprite(PotionDataSO potionData)
    {
        if (potionData != null)
        {
            slotImage.enabled = true;
            potionSprite.sprite = potionData.sprite;
        }
        else
        {
            slotImage.enabled = false;
            potionSprite.sprite = null;
        }
    }

    public void OnPotionSlotClear()
    {
        UpdateSprite(null);
        UpdateLoreTexts(null);
    }

    private bool IsPotionUseAvailable()
    {
        // 인벤토리의 자신의 인덱스에 포션이 없을 경우에도 사용 불가
        if (Inventory.Instance.GetPotionData(index) == null)
        {
            return false;
        }
        // 추가 조건을 더 넣을 수 있음
        return true;
    }

    public void OnClickPotionSlotUI()
    {
        TogglePotionUseUI();
    }

    private void TogglePotionUseUI()
    {
        PotionUseUI.SetActive(!isUseUIActivate);
        isUseUIActivate = !isUseUIActivate;
    }

    public void OnClickPotionUseUI()
    {
        Inventory.Instance.UsePotionInSlots(index);
        TogglePotionUseUI();
    }

    public void OnClickPotionTrashUI()
    {
        Inventory.Instance.EmptyPotionSlot(index);
    }


}
