using UnityEngine;
using System.Collections;

public class MapUI : MonoBehaviour
{
    [SerializeField] private RectTransform MapToggleOnPos;
    [SerializeField] private RectTransform MapToggleOffPos;
    [SerializeField] private RectTransform MapMovingPart;

    private float moveSpeed = 1000f;

    // 현재 맵이 열린 상태인지 여부
    private bool isMapUIOpen = false;
    // 이동 중인지 여부 (이동 중일 때는 버튼 입력 무시)
    private bool isMoving = false;

    public void OnToggleMapButton()
    {
        if (isMoving)
            return;

        Vector2 targetPos = isMapUIOpen ? MapToggleOffPos.anchoredPosition : MapToggleOnPos.anchoredPosition;
        StartCoroutine(MoveMapUI(targetPos));
    }

    private IEnumerator MoveMapUI(Vector2 targetPos)
    {
        isMoving = true;

        Vector2 startPos = MapMovingPart.anchoredPosition;

        float distance = Vector2.Distance(startPos, targetPos);
        float duration = (moveSpeed > 0f) ? distance / moveSpeed : 0.5f;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            MapMovingPart.anchoredPosition = Vector2.Lerp(startPos, targetPos, elapsed / duration);
            yield return null;
        }
        MapMovingPart.anchoredPosition = targetPos;

        isMapUIOpen = !isMapUIOpen;
        isMoving = false;
    }
}
