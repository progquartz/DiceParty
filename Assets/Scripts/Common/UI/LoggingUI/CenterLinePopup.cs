using TMPro;
using UnityEngine;

public class CenterLinePopup : BaseUI
{
    [SerializeField] private SelfDestructFader timer;
    [SerializeField] private TMP_Text centerLineText;

    private void Init()
    {
        RegisterEvents();
        
    }

    private void RegisterEvents()
    {
        timer.OnTimerEnd += OnTimerFinish;
    }

    public void InitializeTexts(string text)
    {
        centerLineText.text = text;
    }

    public void InitializeTimer(float time)
    {
        timer.Init(time);
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
