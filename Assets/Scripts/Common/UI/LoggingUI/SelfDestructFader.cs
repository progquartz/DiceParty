using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelfDestructFader : MonoBehaviour
{
    [Header("���ӽð�")]
    public float fadeDuration = 3.0f;
    private float timer = 0f;

    private bool isTimerStarted = false;
    [SerializeField] private Image[] spriteRenderers;
    [SerializeField] private TMP_Text[] texts;

    // ���� �� ����(���� ����) ����
    [SerializeField] private Color[] initialSpriteColor;
    [SerializeField] private Color[] initialTextColor;

    public event Action OnTimerEnd;

    public void Init(float time)
    {
        fadeDuration = time;
        timer = 0f;
        isTimerStarted = true;

        for(int i = 0; i < spriteRenderers.Length; i++)
        {
            spriteRenderers[i].color = initialSpriteColor[i];
        }

        for(int i = 0; i < texts.Length; i++)
        {
            texts[i].color = initialTextColor[i];
        }
    }

    void Update()
    {
        if(isTimerStarted)
        {
            // ��� �ð� ������Ʈ
            timer += Time.deltaTime;

            // ���� �������� ����(���İ�) ���� ���
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                float alpha = Mathf.Lerp(initialSpriteColor[i].a, 0, timer / fadeDuration);
                spriteRenderers[i].color = new Color(initialSpriteColor[i].r, initialSpriteColor[i].g, initialSpriteColor[i].b, alpha);
            }

            for(int i = 0; i < texts.Length; i++)
            {
                float alpha = Mathf.Lerp(initialTextColor[i].a, 0, timer / fadeDuration);
                texts[i].color = new Color(initialTextColor[i].r, initialTextColor[i].g, initialTextColor[i].b, alpha);
            }


            // ������ �ð��� ������ ������Ʈ ����
            if (timer >= fadeDuration)
            {
                OnTimerEnd?.Invoke();
            }
        }
    }
}
