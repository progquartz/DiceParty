using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenPopup : BaseUI
{
    // ���̵� ��/�ƿ��� ��� Image�� Text �迭
    public Image img;
    public TMP_Text txt;
    [SerializeField] RectTransform canvasRect;

    // ���̵� �ΰ� ���̵� �ƿ��� �ɸ��� �ð� (�� ����)
    public float fadeInTime = 0.5f;
    public float delayInTime = 1.0f;
    public float fadeOutTime = 1.0f;

    public void StartFade(string fadeText, float fadeInTime, float delayInTime, float fadeOutTime)
    {
        SetData(fadeText, fadeInTime, delayInTime, fadeOutTime);
        StartCoroutine(FadeSequence());
    }

    private void SetData(string fadeText, float fadeInTime, float delayTime, float fadeOutTime)
    {
        canvasRect.position = Vector3.zero;
        txt.text = fadeText;
        this.fadeInTime = fadeInTime;
        this.delayInTime = delayTime;
        this.fadeOutTime = fadeOutTime;
    }

    private IEnumerator FadeSequence()
    {
        SetAlpha(0f);
        float timer = 0f;
        while (timer < fadeInTime)
        {
            float alpha = timer / fadeInTime;
            SetAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetAlpha(1f);

        
        yield return new WaitForSeconds(delayInTime);
        timer = 0f;
        while (timer < fadeOutTime)
        {
            float alpha = 1f - (timer / fadeOutTime);
            SetAlpha(alpha);
            timer += Time.deltaTime;
            yield return null;
        }
        SetAlpha(0f);
        onEnd();
    }

    private void SetAlpha(float alpha)
    {
        if (img != null)
        {
            Color col = img.color;
            col.a = alpha;
            img.color = col;
        }
        if (txt != null)
        {
            Color col = txt.color;
            col.a = alpha;
            txt.color = col;
        }
    }

    private void onEnd()
    {
        Debug.Log("���̵� ��/�ƿ� �Ϸ�");
        Close();
    }
}
