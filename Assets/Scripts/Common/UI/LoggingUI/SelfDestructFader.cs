using System;
using UnityEngine;

public class SelfDestructFader : MonoBehaviour
{
    [Header("���ӽð�")]
    public float fadeDuration = 3.0f;
    private float timer = 0f;

    private bool isTimerStarted = false;
    [SerializeField] private SpriteRenderer[] spriteRenderer;

    // ���� �� ����(���� ����) ����
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
            // ��� �ð� ������Ʈ
            timer += Time.deltaTime;

            // ���� �������� ����(���İ�) ���� ���
            for (int i = 0; i < spriteRenderer.Length; i++)
            {
                float alpha = Mathf.Lerp(initialColor[i].a, 0, timer / fadeDuration);
                spriteRenderer[i].color = new Color(initialColor[i].r, initialColor[i].g, initialColor[i].b, alpha);
            }


            // ������ �ð��� ������ ������Ʈ ����
            if (timer >= fadeDuration)
            {
                OnTimerEnd?.Invoke();
            }
        }
    }
}
