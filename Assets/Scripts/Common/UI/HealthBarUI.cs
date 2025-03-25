using TMPro;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private BaseTarget target;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private RectTransform healthBarRect;
    [SerializeField] private RectTransform healthBarOriginalRectSize;
    


    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        UpdateHealthBarSize();
        UpdateHealthBarText();
    }

    private void UpdateHealthBarSize()
    {
        Vector2 originalSize = healthBarOriginalRectSize.sizeDelta;
        float hpRatio = (float)target.stat.Hp / (float)target.stat.maxHp;

        healthBarRect.sizeDelta = new Vector2(originalSize.x * hpRatio, originalSize.y);
    }

    private void UpdateHealthBarText()
    {
        healthText.text = target.stat.Hp + "/" + target.stat.maxHp;
    }
}
