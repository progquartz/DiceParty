using System;
using UnityEngine;

public class BaseUIData
{
    public Action ActionOnShow;
    public Action ActionOnClose;
}

public class BaseUI : MonoBehaviour
{
    public Animation AnimOnOpen;

    private Action _actionOnShow;
    private Action _actionOnClose;

    public virtual void Init(Transform canvas)
    {
        Logger.Log($"{GetType()}::{nameof(Init)}");

        _actionOnShow = null;
        _actionOnClose = null;

        transform.SetParent(canvas);

        var rectTransform = transform as RectTransform;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.localRotation = Quaternion.identity;
        rectTransform.offsetMin = Vector3.zero;
        rectTransform.offsetMax = Vector3.zero;
    }


    public virtual void SetData(BaseUIData data)
    {
        Logger.Log($"{GetType()}::{nameof(SetData)}");

        _actionOnShow = data.ActionOnShow;
        _actionOnClose = data.ActionOnClose;
    }

    public virtual void Show()
    {
        if (AnimOnOpen != null)
        {
            AnimOnOpen.Play();
        }

        _actionOnShow?.Invoke();
        _actionOnShow = null;
    }

    public virtual void Close(bool isCloseAll = false)
    {
        if (isCloseAll == false)
        {
            _actionOnClose?.Invoke();
        }
        _actionOnClose = null;

        UIManager.Instance.CloseUI(this);
    }
}
