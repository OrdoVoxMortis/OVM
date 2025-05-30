using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUI : BaseUI
{
    public override bool IsPopup => true;

    public override void Show()
    {
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        Close();
    }

    public override void Close()
    {
        Debug.Log($"[PopupUI] {name} Close 호출됨 - 파괴처리");
        Destroy(gameObject);
    }
}
