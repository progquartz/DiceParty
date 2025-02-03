using UnityEngine;
using UnityEngine.EventSystems;

public class OnCursorActivation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI 창 오브젝트 (활성화/비활성화 대상)")]
    public GameObject uiWindow; // Inspector에서 활성화할 UI 창을 할당합니다.

    [Header("커서가 머무를 시간 (초)")]
    public float hoverTime = 1.0f; // 커서가 머무르는 최소 시간을 지정합니다.

    private bool isHovering = false;
    private float timer = 0f;

    private void Update()
    {
        // 커서가 버튼 위에 있다면 타이머 증가
        if (isHovering)
        {
            timer += Time.deltaTime;
            if (timer >= hoverTime)
            {
                ActivateUIWindow();
            }
        }
    }

    // 마우스 커서가 버튼 위에 들어왔을 때 호출
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        timer = 0f; // 타이머 초기화
    }

    // 마우스 커서가 버튼에서 나갔을 때 호출
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        timer = 0f;
        // 만약 버튼에서 나가면 UI 창도 비활성화 (원하는 경우)
        DeactivateUIWindow();
    }

    void ActivateUIWindow()
    {
        if (uiWindow != null && !uiWindow.activeSelf)
        {
            uiWindow.SetActive(true);
        }
    }

    void DeactivateUIWindow()
    {
        if (uiWindow != null && uiWindow.activeSelf)
        {
            uiWindow.SetActive(false);
        }
    }
}
