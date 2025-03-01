using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SkillUIInteract : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GraphicRaycaster graphicRaycaster;
    public PointerEventData pointerEventData;
    public EventSystem eventSystem;

    [SerializeField] private SkillUI owner;

    private bool isDragging = false;

    private Vector3 offset;

    private SkillUISlot currentSlot = null;
    

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (BattleManager.Instance.battleState == BattleState.BattleEnd)
        {
            isDragging = true;

            // ���Կ� �ڸ������ ����, ���콺�� �������ν� Detach
            if (currentSlot != null)
            {
                currentSlot.OnSkillUIDetach(owner);
                owner.OnSkillSlotDetach();
                currentSlot = null;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(BattleManager.Instance.battleState == BattleState.BattleEnd)
        {
            isDragging = false;
            bool droppedOnSlot = false;

            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            //Input.mousePosition;
            Debug.Log("z���̳� ����" + pointerEventData.position.ToString());

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            // Raycast ����� ��ġ�� UI Ȯ�� ����
            foreach (var r in results)
            {
                var slot = r.gameObject.GetComponent<SkillUISlot>();
                if (slot != null)
                {
                    // ���Կ� ��ų�� ���� ��쿡
                    if (!slot.HasSkill())
                    {
                        // ���Կ� ��ų �ֱ�.   
                        slot.OnSkillUIAttach(owner);
                        owner.OnSkillSlotAttach(slot);

                        // ���� �� �ֻ����� currentSlot�� ����
                        currentSlot = slot;

                        droppedOnSlot = true;
                        break;
                    }
                    else
                    {
                        Logger.Log("[SkillUIInteract] ��ų�� �̹� �� ��ų ���Կ� ���� �մϴ�.");
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

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
