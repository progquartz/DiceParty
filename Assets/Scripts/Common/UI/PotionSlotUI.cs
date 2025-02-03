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
    // ���� ��ư�� �̹���
    [SerializeField] private Button slotButton;
    [SerializeField] private Image slotImage;
    

    [SerializeField] private bool isUseUIActivate = false;
    public int index;

    private void Update()
    {

        UpdateInteractable(); 
        
        // ���� ������ ������Ʈ
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
        // BattleManager������ ���� �߿����� ��� ������ �Ͱ� ������ ���� �۾� �ʿ�.
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
        // �������� �ƴ� ��쿡�� ���� ��� �Ұ���.
        if(BattleManager.Instance.battleState == BattleState.BattleEnd)
        {
            return false;
        }
        // �κ��丮�� �ڽ��� �ε����� ������ ���� ��쿡�� ��� ����.
        if (Inventory.Instance.GetPotionData(index) == null)
        {
            return false;
        }
        // ���� �߰��� �� ����.
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
