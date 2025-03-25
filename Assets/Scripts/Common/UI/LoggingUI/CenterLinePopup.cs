using TMPro;
using UnityEngine;

public class CenterLinePopup : BaseUI
{
    [SerializeField] private SelfDestructFader timer;
    [SerializeField] private TMP_Text centerLineText;

    public void Init(string text, float appearTime, float fadeTime)
    {
        RegisterEvents();
        InitializeTexts(text);
        InitializeTimer(appearTime,fadeTime);
    }

    private void RegisterEvents()
    {
        timer.OnTimerEnd += OnTimerFinish;
    }

    public void InitializeTexts(string text)
    {
        centerLineText.text = text;
    }

    public void InitializeTimer(float appearTime, float fadeTime)
    {
        timer.Init(appearTime, fadeTime);
    }

    private void ReleaseEvents()
    {
        timer.OnTimerEnd -= OnTimerFinish;
    }

    private void OnTimerFinish()
    {
        ReleaseEvents();
        Close();
    }


}
