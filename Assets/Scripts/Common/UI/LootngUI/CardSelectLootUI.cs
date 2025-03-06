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
        // ��ư �Ʒ��� ����� �� ���� (������) 3���� ī�带 ��ȯ.
        // 3�� �� 1���� Ŭ���� �Ǹ�, 2���� �����.
        // �׸��� ����.
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
        // SkillParent�� ���� ����, �����θ� ����.
        LootingManager.Instance.SkillUiSpawner.SetSkillUIParentToSkillParent(OpenedSkillUI[buttonIndex]);
        Close();
    }

    public void OnClickSkipCardSelection()
    {
        Close();
    }
}
