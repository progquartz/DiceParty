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

        // ���� ������Ʈ�� �����ִ� Canvas�� ã���ϴ�.
        // Hierarchy ������ ���� ������ �������� �˴ϴ�.
        // (���� Drag&Drop�ϰų�, Ȥ�� ���� �θ𿡼� GetComponentInParent<Canvas>() ��)
        canvas = GetComponentInParent<Canvas>();
    }

    // �巡�� ���� �� �� �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        // �巡���ϴ� ���� Raycast ��󿡼� �����ϱ� ����
        // (�ٸ� UI���� ��� Raycast�� ���������� �ޱ� ����)
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
        }
    }

    // �巡�� �� �� �����Ӹ��� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        // Screen Space ���¿� ���缭 ��ġ�� ����
        // (UI ��ǥ �̵��� RectTransformUtility�� ����ϰų�, Anchor � �°� ����)
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

    // �巡�� ���� �� �� �� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        // �ٽ� Raycast�� �޵��� ���� ����
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
        }

        // ���⼭ �ٸ� UI�� ���ƴ��� Ȯ���ϰų�
        // RaycastResult ���� ���� ���Կ� ����ߴ��� ���� üũ
    }
}
