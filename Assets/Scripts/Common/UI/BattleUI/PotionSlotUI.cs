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
        PotionDataSO data = Inventory.Instance.GetPotionData(index);
        // 인벤토리의 자신의 인덱스에 포션이 없을 경우에도 사용 불가
        if (data == null)
        {
            return false;
        }
        // 전투 중 사용이 가능한 경우, 플레이어의 턴에만 사용 가능.
        if(data.isPotionOnlyUsedInBattle && BattleManager.Instance.battleState != BattleStateType.PlayerTurn)
        {
            return false;
        }
        // 비전투 중 사용이 가능한 경우라도, 적 턴 도중에는 사용할 수 없음.
        if(!data.isPotionOnlyUsedInBattle && BattleManager.Instance.battleState == BattleStateType.EnemyTurn)
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
        if(IsPotionUseAvailable())
        {
            Inventory.Instance.UsePotionInSlots(index);
            TogglePotionUseUI();
        }
        else
        {
            // 포션을 사용할 수 없는 상태입니다 표시.
            UIManager.Instance.OpenUI<CenterLinePopup>(new BaseUIData { });
            UIManager.Instance.GetActiveUI<CenterLinePopup>().GetComponent<CenterLinePopup>().Init("You can't use potions right now", 1.0f, 0.5f);
        }
    }

    public void OnClickPotionTrashUI()
    {
        Inventory.Instance.EmptyPotionSlot(index);
    }


}
