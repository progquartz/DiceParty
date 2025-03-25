using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfDestructFader : MonoBehaviour
{
    [Header("���ӽð�")]
    public float appearDuration = 1.0f;
    public float fadeDuration = 3.0f;
    private float timer = 0f;

    private bool isTimerStarted = false;
    [SerializeField] private Image[] spriteRenderers;
    [SerializeField] private TMP_Text[] texts;

    // ���� �� ����(���� ����) ����
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
            // ��� �ð� ������Ʈ
            timer += Time.deltaTime;

            if (timer > appearDuration)
            {
                // ���� �������� ����(���İ�) ���� ���
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
            // ������ �ð��� ������ ������Ʈ ����
            if (timer >= appearDuration + fadeDuration)
            {
                OnTimerEnd?.Invoke();
            }
        }
    }
}
