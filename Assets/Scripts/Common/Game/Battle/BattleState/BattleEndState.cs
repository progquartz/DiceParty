using UnityEngine;

public class BattleEndState : BattleState
{
    private bool isPlayerWin;

    public BattleEndState(BattleManager battleManager, bool isPlayerWin) : base(battleManager)
    {
        this.isPlayerWin = isPlayerWin;
    }

    public override void Enter()
    {
        Debug.Log($"���� ���� - {(isPlayerWin ? "�÷��̾� �¸�" : "�÷��̾� �й�")}");
        battleManager.battleState = BattleStateType.BattleEnd;
        

        // �ֻ��� �ʱ�ȭ
        battleManager.ResetDiceToDummy();

        if (isPlayerWin)
        {
            HandlePlayerVictory();
        }
        else
        {
            HandlePlayerDefeat();
        }

        battleManager.prevBattleType = battleManager.currentBattleType;
        battleManager.currentBattleType = BattleType.None;
    }

    public override void Exit()
    {
        // ���� ���� ���¿����� Ư���� ���� ó���� �ʿ� ����
    }

    private void HandlePlayerVictory()
    {
        Debug.LogWarning("�� ��Ʋ�ʵ� ���� ����!");
        // �� ��Ʋ�ʵ� ����
        battleManager.ClearEnemyBattlefield();

        // ���� ����
        if ((int)battleManager.currentBattleType % 10 == 2)
        {
            LootingManager.Instance.OpenLootingTable(battleManager.currentBattleType, true);
        }
        else
        {
            LootingManager.Instance.OpenLootingTable(battleManager.currentBattleType, false);
        }
    }

    private void HandlePlayerDefeat()
    {
        // �÷��̾� ĳ���� ����
        battleManager.ClearPlayerBattlefield();

        UIManager.Instance.OpenUI<FullScreenPopup>(new BaseUIData { });
        FullScreenPopup nextStagePopup = UIManager.Instance.GetActiveUI<FullScreenPopup>().GetComponent<FullScreenPopup>();
        nextStagePopup.StartFade($"- Game Over -", 0.5f, 1000f, 1.0f);
        // ���ӿ��� UI ǥ�� ���� ó��
        // TODO: ���ӿ��� ó�� �߰�
    }
}
