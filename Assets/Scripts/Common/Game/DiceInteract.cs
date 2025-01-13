using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceInteract : MonoBehaviour
{

    private bool isDragging = false;
    private Vector3 offset;
    private Dice dice;

    // ★ 이 부분을 Inspector에서 연결해야 함 (Canvas에 있는 GraphicRaycaster)
    public GraphicRaycaster graphicRaycaster;
    // 마우스 이벤트를 담을 EventSystem (씬에 있어야 함)
    public EventSystem eventSystem;

    public void Init()
    {
        graphicRaycaster = UIManager.GetRayCaster();
        dice = GetComponent<Dice>();
    }

    private void Awake()
    {
        Init();
    }

    void Update()
    {
        if (isDragging)
        {
            // 마우스 위치를 월드 좌표로 변환
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;  // 2D이므로 z는 고정
            transform.position = mousePos + offset;
        }
    }

    // 주사위 드래그 시작
    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        offset = transform.position - mousePos;
    }

    // 드래그 중지
    void OnMouseUp()
    {
        isDragging = false;

        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        // raycast로 resultData 가져오기
        graphicRaycaster.Raycast(pointerData, results);

        bool droppedOnSlot = false;

        foreach (var r in results)
        {
            var slot = r.gameObject.GetComponent<SkillDiceSlotUI>();
            if (slot != null)
            {
                // 슬롯 스크립트가 있다면, 드롭 성공으로 판단
                slot.OnDiceDropped(dice);
                droppedOnSlot = true;
                break;
            }
        }

        if (droppedOnSlot)
        {
            Debug.Log("슬롯에 드롭 성공!");
            
        }
        else
        {
            Debug.Log("슬롯이 없는 곳에 드롭됨.");
        }
    }
}
