using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfDestructFader : MonoBehaviour
{
    [Header("지속시간")]
    public float appearDuration = 1.0f;
    public float fadeDuration = 3.0f;
    private float timer = 0f;

    private bool isTimerStarted = false;
    [SerializeField] private Image[] spriteRenderers;
    [SerializeField] private TMP_Text[] texts;

    // 시작 시 색상(투명도 포함) 저장
    [SerializeField] private Color[] initialSpriteColor;
    [SerializeField] private Color[] initialTextColor;

    public event Action OnTimerEnd;

    public void Init(float appearTime, float fadeTime)
    {
        appearDuration = appearTime;
        fadeDuration = fadeTime;
        timer = 0f;
        

        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = initialSpriteColor[i];
        }

        for(int i = 0; i < texts.Length; i++)
        {
            texts[i].color = initialTextColor[i];
        }

        isTimerStarted = true;
    }

    void Update()
    {
        if(isTimerStarted)
        {
            // 경과 시간 업데이트
            timer += Time.deltaTime;

            if (timer > appearDuration)
            {
                // 선형 보간으로 투명도(알파값) 감소 계산
                for (int i = 0; i < spriteRenderers.Length; i++)
                {
                    float alpha = Mathf.Lerp(initialSpriteColor[i].a, 0, timer - appearDuration / fadeDuration);
                    spriteRenderers[i].color = new Color(initialSpriteColor[i].r, initialSpriteColor[i].g, initialSpriteColor[i].b, alpha);
                }

                for (int i = 0; i < texts.Length; i++)
                {
                    float alpha = Mathf.Lerp(initialTextColor[i].a, 0, timer - appearDuration / fadeDuration);
                    texts[i].color = new Color(initialTextColor[i].r, initialTextColor[i].g, initialTextColor[i].b, alpha);
                }
            }
            // 지정된 시간이 지나면 오브젝트 삭제
            if (timer >= appearDuration + fadeDuration)
            {
                OnTimerEnd?.Invoke();
            }
        }
    }
}
