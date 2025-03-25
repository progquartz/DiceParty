using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDragTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        // 현재 오브젝트가 속해있는 Canvas를 찾습니다.
        // Hierarchy 상에서 가장 가까운 부모에서 찾습니다.
        // (직접 Drag&Drop하거나, 혹은 가장 부모에서 GetComponentInParent<Canvas>() 등)
        canvas = GetComponentInParent<Canvas>();
    }

    // 드래그 시작 할 때 한 번 호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 드래그하는 동안 Raycast 감지에서 제외하기 위함
        // (다른 UI들이 모든 Raycast를 정상적으로 받기 위함)
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    // 드래그 중 매 프레임마다 호출
    public void OnDrag(PointerEventData eventData)
    {
        // Screen Space 상태에 맞춰서 위치를 조정
        // (UI 좌표 이동은 RectTransformUtility를 사용하거나, Anchor에 맞게 조정)
        if (canvas == null) return;

        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out pos
        );

        rectTransform.anchoredPosition = pos;
    }

    // 드래그 종료 할 때 한 번 호출
    public void OnEndDrag(PointerEventData eventData)
    {
        // 다시 Raycast를 받도록 설정 복구
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        // 여기서 다른 UI에 닿았는지 확인하거나
        // RaycastResult 등을 통해 어디에 도달했는지 등을 체크
    }
}
