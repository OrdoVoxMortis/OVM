using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_GameClear : BaseUI
{
    StageResult stage;

    [SerializeField] private TextMeshProUGUI missionDialog;
    [SerializeField] private TextMeshProUGUI planDialog;
    [SerializeField] private TextMeshProUGUI eventDialog;
    [SerializeField] private TextMeshProUGUI rhythmDialog;

    protected override void Awake()
    {
        stage = FindAnyObjectByType<StageResult>();
        GameManager.Instance.OnGameOver += SetText;
    }

    public void SetText()
    {
        missionDialog.text = stage.missionDialog;
        if (string.IsNullOrEmpty(missionDialog.text))
        {
            Destroy(missionDialog.transform.parent.gameObject);
        }

        planDialog.text = stage.planDialog;
        if (string.IsNullOrEmpty(planDialog.text))
        {
            Destroy(planDialog.transform.parent.gameObject);
        }

        eventDialog.text = stage.eventDialog;
        if (string.IsNullOrEmpty(eventDialog.text))
        {
            Destroy(eventDialog.transform.parent.gameObject);
        }

        rhythmDialog.text = stage.rhythmDialog;
        if (string.IsNullOrEmpty(rhythmDialog.text))
        {
            Destroy(rhythmDialog.transform.parent.gameObject);
        }
    }
}
