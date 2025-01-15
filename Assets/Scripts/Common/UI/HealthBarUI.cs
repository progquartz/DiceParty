using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private BaseTarget target;
    [SerializeField] private RectTransform healthBarRect;
    [SerializeField] private RectTransform healthBarOriginalRectSize;


    private void Update()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        Vector2 originalSize = healthBarOriginalRectSize.sizeDelta;
        float hpRatio = (float)target.Hp / (float)target.maxHp;

        healthBarRect.sizeDelta = new Vector2 (originalSize.x * hpRatio, originalSize.y);
    }
}
