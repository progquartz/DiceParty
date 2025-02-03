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
    // 슬롯 버튼과 이미지
    [SerializeField] private Button slotButton;
    [SerializeField] private Image slotImage;
    

    [SerializeField] private bool isUseUIActivate = false;
    public int index;

    private void Update()
    {

        UpdateInteractable(); 
        
        // 포션 데이터 업데이트
        PotionDataSO potionData = Inventory.Instance.GetPotionData(index);
        UpdateSprite(potionData);
        UpdateLoreTexts(potionData);
    }

    private void UpdateLoreTexts(PotionDataSO potionData)
    {
        if(potionData != null)
        {
            potionNameText.text = potionData.name;
            potionLoreText.text = potionData.lore;
        }
    }

    private void UpdateInteractable()
    {
        // BattleManager에서의 전투 중에서만 사용 가능한 것과 연관된 관련 작업 필요.
        if (CheckAvailability())
        {
            slotButton.interactable = true;
        }
        else
        {
            if (isUseUIActivate)
            {
                TogglePotionUseUI();
            }
            slotButton.interactable = false;
        }
    }

    private bool CheckAvailability()
    {
        // 전투중이 아닐 경우에는 포션 사용 불가능.
        if(BattleManager.Instance.battleState == BattleState.BattleEnd)
        {
            return false;
        }
        // 인벤토리의 자신의 인덱스에 포션이 없을 경우에는 사용 못함.
        if (Inventory.Instance.GetPotionData(index) == null)
        {
            return false;
        }
        // 조건 추가될 수 있음.
        return true;

    }

    public void OnClickPotionSlotUI()
    {
        if(CheckAvailability() )
        {
            TogglePotionUseUI();
        }
    }

    private void TogglePotionUseUI()
    {
        PotionUseUI.SetActive(!isUseUIActivate);
        isUseUIActivate = !isUseUIActivate;
    }

    public void OnClickPotionUseUI()
    {
        Inventory.Instance.UsePotionInSlots(index);
    }

    public void OnClickPotionTrashUI()
    {
        Inventory.Instance.EmptyPotionSlot(index);
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
}
