using UnityEngine;
using System.Collections;
using TMPro;

public class MapUI : MonoBehaviour
{
    [Header("맵 부분")]
    [SerializeField] private RectTransform MapToggleOnPos;
    [SerializeField] private RectTransform MapToggleOffPos;
    [SerializeField] private RectTransform MapMovingPart;
    private float openSpeed = 1000f;
    private float closeSpeed = 1500f;
    private float openSpeedFast = 2000f;
    private float closeSpeedFast = 3000f;

    // 현재 맵이 열려 있는지의 여부
    private bool isMapUIOpen = false;
    // 이동 중인지 여부 (이동 중일 때는 버튼 입력 무시)
    private bool isMoving = false;


    [Header("맵 팝업 부분")]
    [SerializeField] private RectTransform MapCantReachUITransform;
    [SerializeField] private TMP_Text MapCantReachUIText;

    private float mapCantReachUIDuration = 1.5f;
    private bool isToggleOpened = false;


    public void OnToggleReachUI(string toggleCallString)
    {
        if(isToggleOpened)
        {
            Logger.Log("이미 맵 팝업이 열려있는 상태입니다.");
            return;
        }
        MapCantReachUIText.text = toggleCallString;
        StartCoroutine(OpenMapCantReachUI());
    }

    public void OnTurningOffMapUI()
    {
        if (isMoving || !isMapUIOpen)
            return;

        Vector2 targetPos = MapToggleOffPos.anchoredPosition;
        StartCoroutine(MoveMapUI(targetPos, true));
    }
    public void OnToggleMapUITeleport()
    {
        if (isMoving)
            return;
        Vector2 targetPos = isMapUIOpen ? MapToggleOffPos.anchoredPosition : MapToggleOnPos.anchoredPosition;
        MapMovingPart.anchoredPosition = targetPos;
    }

    public void OnTurningOffMapUITeleport()
    {
        if (isMoving)
            return;
        Vector2 targetPos = MapToggleOffPos.anchoredPosition;
        MapMovingPart.anchoredPosition = targetPos;
    }

    public void OnToggleMapButton(bool isFast)
    {
        if (isMoving)
            return;

        Vector2 targetPos = isMapUIOpen ? MapToggleOffPos.anchoredPosition : MapToggleOnPos.anchoredPosition;
        StartCoroutine(MoveMapUI(targetPos, isFast));
    }

    private IEnumerator MoveMapUI(Vector2 targetPos, bool isFast)
    {
        isMoving = true;

        Vector2 startPos = MapMovingPart.anchoredPosition;

        float distance = Vector2.Distance(startPos, targetPos);
        float duration;
        if (isFast)
        {
            if(isMapUIOpen)
                duration = (closeSpeed > 0f) ? distance / closeSpeed : 0.5f;
            else
                duration = (openSpeed > 0f) ? distance / openSpeed : 0.5f;
        }
        else
        {
            if(isMapUIOpen)
                duration = (closeSpeedFast > 0f) ? distance / closeSpeedFast : 0.5f;
            else
                duration = (openSpeedFast > 0f) ? distance / openSpeedFast : 0.5f;
        }
        
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

    private IEnumerator OpenMapCantReachUI()
    {
        isToggleOpened = true;
        MapCantReachUITransform.gameObject.SetActive(true);
        float elapsed = 0f;
        while(elapsed < mapCantReachUIDuration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        MapCantReachUITransform.gameObject.SetActive(false);
        isToggleOpened = false;
    }


}
