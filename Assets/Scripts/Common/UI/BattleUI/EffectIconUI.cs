using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EffectIconUI : MonoBehaviour
{
    public EffectKey effectCode;

    [SerializeField] private TMP_Text effectIconName;
    [SerializeField] private TMP_Text effectIconLore;

    [SerializeField] private Image effectIcon;

    private void Awake()
    {
        
    }

    private void Init(EffectKey effect)
    {
        
    }
}
