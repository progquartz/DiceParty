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
        // 버튼 아래의 스테이지 별 등급 (랜덤한) 3개의 카드를 생성.
        // 3개 중 1개를 클릭을 하면, 2개는 사라짐.
        // 그리고 종료.
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
        // SkillParent로 부모 변경, 나머지는 삭제.
        LootingManager.Instance.SkillUiSpawner.SetSkillUIParentToSkillParent(OpenedSkillUI[buttonIndex]);
        Close();
    }

    public void OnClickSkipCardSelection()
    {
        Close();
    }
}
