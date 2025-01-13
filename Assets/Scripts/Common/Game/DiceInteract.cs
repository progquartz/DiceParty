using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceInteract : MonoBehaviour
{

    private bool isDragging = false;
    private Vector3 offset;
    private Dice dice;

    // �� �� �κ��� Inspector���� �����ؾ� �� (Canvas�� �ִ� GraphicRaycaster)
    public GraphicRaycaster graphicRaycaster;
    // ���콺 �̺�Ʈ�� ���� EventSystem (���� �־�� ��)
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
            // ���콺 ��ġ�� ���� ��ǥ�� ��ȯ
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;  // 2D�̹Ƿ� z�� ����
            transform.position = mousePos + offset;
        }
    }

    // �ֻ��� �巡�� ����
    void OnMouseDown()
    {
        isDragging = true;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        offset = transform.position - mousePos;
    }

    // �巡�� ����
    void OnMouseUp()
    {
        isDragging = false;

        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();

        // raycast�� resultData ��������
        graphicRaycaster.Raycast(pointerData, results);

        bool droppedOnSlot = false;

        foreach (var r in results)
        {
            var slot = r.gameObject.GetComponent<SkillDiceSlotUI>();
            if (slot != null)
            {
                // ���� ��ũ��Ʈ�� �ִٸ�, ��� �������� �Ǵ�
                slot.OnDiceDropped(dice);
                droppedOnSlot = true;
                break;
            }
        }

        if (droppedOnSlot)
        {
            Debug.Log("���Կ� ��� ����!");
            
        }
        else
        {
            Debug.Log("������ ���� ���� ��ӵ�.");
        }
    }
}
