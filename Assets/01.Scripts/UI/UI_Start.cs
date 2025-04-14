using UnityEngine;
using UnityEngine.UI;

public class UI_Start: BaseUI
{
    [SerializeField] private Button closeBtn; // 연결할 버튼

    protected override void Awake()
    {
        base.Awake();

        if(closeBtn != null)
         closeBtn.onClick.AddListener(Hide);
    }
}
