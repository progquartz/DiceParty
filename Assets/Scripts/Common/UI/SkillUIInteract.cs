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

            // 슬롯에 자리잡았을 때에, 마우스를 놓음으로써 Detach
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
            Debug.Log("z값이나 보자" + pointerEventData.position.ToString());

            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerEventData, results);

            // Raycast 결과로 겹치는 UI 확인 가능
            foreach (var r in results)
            {
                var slot = r.gameObject.GetComponent<SkillUISlot>();
                if (slot != null)
                {
                    // 슬롯에 스킬이 없을 경우에
                    if (!slot.HasSkill())
                    {
                        // 슬롯에 스킬 넣기.   
                        slot.OnSkillUIAttach(owner);
                        owner.OnSkillSlotAttach(slot);

                        // 이제 이 주사위의 currentSlot도 갱신
                        currentSlot = slot;

                        droppedOnSlot = true;
                        break;
                    }
                    else
                    {
                        Logger.Log("[SkillUIInteract] 스킬이 이미 찬 스킬 슬롯에 들어가려 합니다.");
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

    public void OnDrag(PointerEventData eventData)
    {
        
    }
}
