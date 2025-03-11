using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class EffectUI : MonoBehaviour
{
    public GameObject effectIconPrefab; // ���� ������ ������
    public Transform effectGrid; // GridLayoutGroup�� ����� UI �г�

    [SerializeField] private BaseTarget target;
    private Dictionary<EffectKey, EffectIconUI> activeEffects = new Dictionary<EffectKey, EffectIconUI>();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        if(target == null)
        {
            target = GetComponentInParent<BaseTarget>();
            Debug.Log("EffectUI�� ������ ã�� ���� �˻��غ��ϴ�.");
            if(target == null ) 
            {
                Debug.Log("����!");
            }
        }
        RegisterEvent();
    }

    private void RegisterEvent()
    {
        target.stat.OnUpdatingEffectStack += AddEffect;
        target.stat.OnRemovingEffectStack += RemoveEffect;
    }

    private void ReleaseEvent()
    {
        target.stat.OnUpdatingEffectStack -= AddEffect;
        target.stat.OnRemovingEffectStack -= RemoveEffect;
    }

    public void RemoveEffect(EffectKey key)
    {
        if(activeEffects.ContainsKey(key) && EffectNeedsBuffIcon(key))
        {
            Destroy(activeEffects[key].gameObject);
            activeEffects.Remove(key);
        }
    }
    public void AddEffect(EffectKey key, int stack)
    {
        if (activeEffects.ContainsKey(key) && EffectNeedsBuffIcon(key))
        {
            activeEffects[key].UpdateVisual(key, stack, 0);
        }
        else
        {
            GameObject iconObject = Instantiate(effectIconPrefab, effectGrid);
            EffectIconUI iconUI = iconObject.GetComponent<EffectIconUI>();
            iconUI.UpdateVisual(key, stack, 0);
            activeEffects[key] = iconUI;
        }
    }

    private bool EffectNeedsBuffIcon(EffectKey key)
    {
        if ((int)key % 10 == 2)
        { 
            return true;
        }
        return false;
    }
    private void OnDestroy()
    {
        ReleaseEvent();
    }
}
