using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceInteract : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Dice dice;

    // �� �ֻ����� ���� � ���Կ� �پ� �ִ���(���ٸ� null)
    private SkillDiceSlotUI currentSlot = null;

    // Canvas�� �ִ� GraphicRaycaster�� EventSystem
    public GraphicRaycaster graphicRaycaster;
    public EventSystem eventSystem;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        if (graphicRaycaster == null)
        {
            graphicRaycaster = UIManager.Instance.GetRayCaster();
        }
        eventSystem = eventSystem ?? EventSystem.current;

        if (dice == null)
            dice = GetComponent<Dice>();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            transform.position = mousePos + offset;
        }
    }

    // �ֻ��� �巡�� ����
    void OnMouseDown()
    {
        isDragging = true;

        // ���Կ� �ڸ������ ����, ���콺�� �������ν� Detach
        if (currentSlot != null)
        {
            currentSlot.OnDiceDetach(dice);
            currentSlot = null;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        offset = transform.position - mousePos;
    }

    // �巡�� ����
    void OnMouseUp()
    {
        isDragging = false;

        // ���콺�� ���� UI�� Raycast�� Ȯ��
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerData, results);

        bool droppedOnSlot = false;

        foreach (var r in results)
        {
            var slot = r.gameObject.GetComponent<SkillDiceSlotUI>();
            if (slot != null)
            {
                // ���Կ� ��ϵ� ���̽��� ���� ��쿡��
                if(slot.IsSlotAvailableToDice())
                {
                    // ���� ��ũ��Ʈ�� �ִٸ� ���Կ� ����
                    slot.OnDiceAttach(dice);

                    // ���� �� �ֻ����� currentSlot�� ����
                    currentSlot = slot;

                    droppedOnSlot = true;
                    break;
                }
                else
                {
                    Logger.Log("[DiceInteract] �ֻ����� ��� �Ұ����� ���Կ� ������ �մϴ�.");
                }

            }
        }

        if (droppedOnSlot)
        {
            Logger.Log("[DiceInteract] ���Կ� ��� ����!");
        }
        else
        {
            Logger.Log("[DiceInteract] ������ �ƴ� ���� ��ӵ�.");
        }
    }
}
