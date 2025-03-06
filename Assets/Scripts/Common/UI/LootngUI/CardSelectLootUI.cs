using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectLootUI : BaseUI
{
    [SerializeField] private List<Transform> CardPos;
    [SerializeField] private List<SkillUI> OpenedSkillUI;

    private void Awake()
    {
        SpawnCard();
        // 버튼 아래로 사용할 수 없는 (가려진) 3개의 카드를 소환.
        // 3개 중 1개가 클릭이 되면, 2개가 사라짐.
        // 그리고 닫힘.
    }

    private void SpawnCard()
    {
        for(int i = 0; i < CardPos.Count; i++) 
        {
            LootingCardSO skillData = LootingManager.Instance.GetRandomLootingCard(MapManager.Instance.currentStageNum);
            SkillUI spawnedUI = LootingManager.Instance.SkillUiSpawner.SpawnSkillUI(skillData.lootingSkillDataSO);
            spawnedUI.transform.parent = CardPos[i].transform;
            spawnedUI.transform.localPosition = Vector3.zero;
            spawnedUI.transform.localScale = Vector3.one;
            OpenedSkillUI.Add(spawnedUI);
        }
    }

    public void OnClickCardSelection(int  buttonIndex)
    {
        // SkillParent로 보낸 다음, 스스로를 삭제.
        LootingManager.Instance.SkillUiSpawner.SetSkillUIParentToSkillParent(OpenedSkillUI[buttonIndex]);
        Close();
    }

    public void OnClickSkipCardSelection()
    {
        Close();
    }
}
