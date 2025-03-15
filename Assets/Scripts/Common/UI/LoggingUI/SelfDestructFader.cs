using System;
using UnityEngine;

public class SelfDestructFader : MonoBehaviour
{
    [Header("지속시간")]
    public float fadeDuration = 3.0f;
    private float timer = 0f;

    private bool isTimerStarted = false;
    [SerializeField] private SpriteRenderer[] spriteRenderer;

    // 시작 시 색상(투명도 포함) 저장
    private Color[] initialColor;

    public event Action OnTimerEnd;

    public void Init(float time)
    {
        fadeDuration = time;
        isTimerStarted = true;
        for(int i = 0; i < spriteRenderer.Length; i++)
        {
            initialColor[i] = spriteRenderer[i].color;
        }
    }

    void Update()
    {
        if(isTimerStarted)
        {
            // 경과 시간 업데이트
            timer += Time.deltaTime;

            // 선형 보간으로 투명도(알파값) 감소 계산
            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                float alpha = Mathf.Lerp(initialColor[i].a, 0, timer / fadeDuration);
                spriteRenderer[i].color = new Color(initialColor[i].r, initialColor[i].g, initialColor[i].b, alpha);
            }


            // 지정된 시간이 지나면 오브젝트 삭제
            if (timer >= fadeDuration)
            {
                OnTimerEnd?.Invoke();
            }
        }
    }
}
