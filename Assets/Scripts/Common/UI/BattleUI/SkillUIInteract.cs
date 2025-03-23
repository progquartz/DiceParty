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
        if (BattleManager.Instance.battleState == BattleStateType.BattleEnd)
        {
            isDragging = true;

            // 슬롯에 자리잡고있던 스킬, 마우스로 드래그함으로써 Detach
            if (owner.skillUISlot != null)
            {
                owner.skillUISlot.OnSkillUIDetach(owner);
                owner.OnSkillSlotDetach();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(BattleManager.Instance.battleState == BattleStateType.BattleEnd)
        {
            isDragging = false;
            bool droppedOnSlot = false;

            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            // Raycast 결과로 부딪힌 UI 확인 작업
            foreach (var r in results)
            {
                var slot = r.gameObject.GetComponent<SkillUISlot>();
                if (slot != null)
                {
                    // 슬롯에 스킬이 없을 경우에
                    if (!slot.HasSkill())
                    {
                        // 슬롯에 스킬 넣기
                        slot.OnSkillUIAttach(owner);
                        owner.OnSkillSlotAttach(slot);
                        droppedOnSlot = true;
                        break;
                    }
                    else
                    {
                        Logger.Log("[SkillUIInteract] 스킬이 이미 찬 스킬 슬롯에 넣을 수 없습니다.");
                    }
                }
            }

            if (droppedOnSlot)
            {
                Logger.Log("[DiceInteract] 슬롯에 부착 성공!");
            }
            else
            {
                //Logger.Log("[DiceInteract] 슬롯이 아닌 곳에 놓아짐.");
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
