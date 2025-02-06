using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceInteract : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Dice dice;

    // 이 주사위가 현재 어떤 슬롯에 붙어 있는지(없다면 null)
    private SkillDiceSlotUI currentSlot = null;

    // Canvas에 있는 GraphicRaycaster와 EventSystem
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

    // 주사위 드래그 시작
    void OnMouseDown()
    {
        isDragging = true;

        // 슬롯에 자리잡았을 때에, 마우스를 놓음으로써 Detach
        if (currentSlot != null)
        {
            currentSlot.OnDiceDetach(dice);
            currentSlot = null;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        offset = transform.position - mousePos;
    }

    // 드래그 중지
    void OnMouseUp()
    {
        isDragging = false;

        // 마우스가 놓인 UI를 Raycast로 확인
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
                // 슬롯에 등록된 다이스가 없는 경우에만
                if(slot.IsSlotAvailableToDice())
                {
                    // 슬롯 스크립트가 있다면 슬롯에 장착
                    slot.OnDiceAttach(dice);

                    // 이제 이 주사위의 currentSlot도 갱신
                    currentSlot = slot;

                    droppedOnSlot = true;
                    break;
                }
                else
                {
                    Logger.Log("[DiceInteract] 주사위가 사용 불간으한 슬롯에 들어가려고 합니다.");
                }

            }
        }

        if (droppedOnSlot)
        {
            Logger.Log("[DiceInteract] 슬롯에 드롭 성공!");
        }
        else
        {
            Logger.Log("[DiceInteract] 슬롯이 아닌 곳에 드롭됨.");
        }
    }
}
