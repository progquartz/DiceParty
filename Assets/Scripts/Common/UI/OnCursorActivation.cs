using UnityEngine;
using UnityEngine.EventSystems;

public class OnCursorActivation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI â ������Ʈ (Ȱ��ȭ/��Ȱ��ȭ ���)")]
    public GameObject uiWindow; // Inspector���� Ȱ��ȭ�� UI â�� �Ҵ��մϴ�.

    [Header("Ŀ���� �ӹ��� �ð� (��)")]
    public float hoverTime = 1.0f; // Ŀ���� �ӹ����� �ּ� �ð��� �����մϴ�.

    private bool isHovering = false;
    private float timer = 0f;

    private void Update()
    {
        // Ŀ���� ��ư ���� �ִٸ� Ÿ�̸� ����
        if (isHovering)
        {
            timer += Time.deltaTime;
            if (timer >= hoverTime)
            {
                ActivateUIWindow();
            }
        }
    }

    // ���콺 Ŀ���� ��ư ���� ������ �� ȣ��
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        timer = 0f; // Ÿ�̸� �ʱ�ȭ
    }

    // ���콺 Ŀ���� ��ư���� ������ �� ȣ��
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        timer = 0f;
        // ���� ��ư���� ������ UI â�� ��Ȱ��ȭ (���ϴ� ���)
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
